using System;
using System.Linq;
using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.PagedListLite;
using log4net;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;
using FriendlyUrlForMvc.Web.Infrastructure.Dto;
using FriendlyUrlForMvc.Web.Infrastructure.QueryParams;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services {

    /// <summary>
    /// Log service interface
    /// </summary>
    public interface ILogService {

        /// <summary>
        /// Log to system event jornal message of Info
        /// </summary>
        /// <param name="message">message text</param>
        void LogInfo(string message);

        /// <summary>
        /// Log to system event jornal message of Info
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="args">objects as params for formating</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Log Error message
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);

        /// <summary>
        /// Log Error message with formating params
        /// </summary>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Log Error message with formating params
        /// </summary>
        /// <param name="exception"></param>
        void LogError(ArgumentNullException exception);

        /// <summary>
        /// Returns paged list of the LogItem
        /// </summary>
        /// <param name="queryParams">QueryParams</param>
        /// <returns></returns>
        OperationResult<IPagedList<LogItemDto>> GetPagedResult(PagedListQueryParams queryParams);

        /// <summary>
        /// Save logItem to database
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="forceSaveToDatabase"></param>
        bool SaveToDatabase(LogType logType, string message, bool forceSaveToDatabase = false);

        /// <summary>
        /// Clear database data with system events
        /// </summary>
        /// <returns></returns>
        OperationResult<bool> ClearResult();

        /// <summary>
        /// Log error as exception
        /// </summary>
        /// <param name="error"></param>
        void LogError(Exception error);
    }

    /// <summary>
    /// Log service is intended for saving log item to log-file and/or database
    /// </summary>
    public class LogService : ILogService {

        private readonly ILog _logger;
        private readonly IContext _context;
        private readonly IAppConfig _config;
        private readonly IMapper _mapper;

        public LogService(ILog logger, IContext context, IAppConfig config, IMapper mapper) {
            _logger = logger;
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        /// <summary>
        /// Log to system event jornal message of Info
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="args">objects as params for formating</param>
        public void LogInfo(string message, params object[] args) {
            _logger.InfoFormat(message, args);
            SaveToDatabase(LogType.Info, string.Format(message, args));
        }

        /// <summary>
        /// Log to system event jornal message of Info
        /// </summary>
        /// <param name="message">message text</param>
        public void LogInfo(string message) {
            _logger.Info(message);
            SaveToDatabase(LogType.Info, message);
        }

        /// <summary>
        /// Log Error message with formating params
        /// </summary>
        public void LogError(string message, params object[] args) {
            _logger.ErrorFormat(message, args);
            SaveToDatabase(LogType.Info, string.Format(message, args));
        }

        /// <summary>
        /// Log Error message with formating params
        /// </summary>
        /// <param name="exception"></param>
        public void LogError(ArgumentNullException exception) {
            _logger.Error(exception);
            SaveToDatabase(LogType.Info, exception.Message);
        }

        /// <summary>
        /// Log Error message
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message) {
            _logger.Error(message);
            SaveToDatabase(LogType.Info, message);
        }

        /// <summary>
        /// Log error as exception
        /// </summary>
        /// <param name="error"></param>
        public void LogError(Exception error) {
            _logger.Error(error);
            SaveToDatabase(LogType.Error, error.Message);
        }

        /// <summary>
        /// Returns paged list of the LogItem
        /// </summary>
        /// <param name="queryParams">QueryParams</param>
        /// <returns></returns>
        public OperationResult<IPagedList<LogItemDto>> GetPagedResult(PagedListQueryParams queryParams) {
            var result = OperationResult.CreateResult<IPagedList<LogItemDto>>();
            if (queryParams == null) {
                result.Error = new ArgumentNullException(nameof(queryParams));
                return result;
            }

            var all = _context.Logs.OrderByDescending(x => x.Id).AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.Search)) {
                try {
                    var searhTerm = queryParams.Search.ToLowerInvariant();

                    all = all.Where(x => x.LogType.ToString().ToLower().StartsWith(searhTerm.ToLower())
                    || x.Message.ToLower().StartsWith(searhTerm.ToLower())
                    || x.StackTrace.ToLower().Contains(searhTerm.ToLower()));
                }
                catch (Exception exception) {
                    result.Error = exception;
                    return result;
                }
            }
            var paged = all.TakePage(queryParams.PageIndex, queryParams.PageSize);
            if (paged.Items.Any()) {
                result.Result = _mapper.Map<PagedList<LogItemDto>>(paged);
                return result;
            }
            result.AddWarning("Не найдено записей в журнале действий");
            return result;
        }

        /// <summary>
        /// Save logItem to database
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="forceSaveToDatabase"></param>
        public bool SaveToDatabase(LogType logType, string message, bool forceSaveToDatabase = false) {
            if (forceSaveToDatabase || _config.Config.IsLogging) {
                _context.Logs.Add(new LogItem {
                    CreatedAt = DateTime.Now,
                    LogType = logType,
                    Message = message
                });
                return
                _context.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// Clear database data with system events
        /// </summary>
        /// <returns></returns>
        public OperationResult<bool> ClearResult() {
            var operation = OperationResult.CreateResult<bool>();
            if (_context.Database.ExecuteSqlCommand("truncate table ApplicationLogItem") != 0) {
                var message = "Журнал событий очищен";
                _context.Logs.Add(new LogItem {
                    CreatedAt = DateTime.Now,
                    LogType = LogType.Info,
                    Message = message
                });
                _context.SaveChanges();
                operation.Result = true;
                operation.AddSuccess(message);
                return operation;


            }
            operation.AddError("Some errors occurred while  CLEAR EVENTS LOG operation execution");
            return operation;
        }
    }
}
