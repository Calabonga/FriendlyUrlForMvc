using System;
using System.Web.Mvc;
using FriendlyUrlForMvc.Web.Infrastructure;
using FriendlyUrlForMvc.Web.Infrastructure.Repository;
using FriendlyUrlForMvc.Web.Models;

namespace FriendlyUrlForMvc.Web.Controllers {

    /// <summary>
    /// Controler to process EditablePage entity
    /// </summary>
    public class SiteController : Controller {

        private readonly EditablePageRepository _editablePageRepository;

        public SiteController(EditablePageRepository editablePageRepository) {
            _editablePageRepository = editablePageRepository;

        }

        #region View
        public ActionResult View(int id) {
            var operationResult = _editablePageRepository.GetById(id);
            if (operationResult.Ok) {
                return View("ViewEditablePage", operationResult.Result);
            }
            return View("404");
        }
        #endregion

        #region Edit

        public ActionResult Edit(int id) {
            var operationResult = _editablePageRepository.GetEditById(id);
            if (operationResult.Ok) {
                return View("EditEditablePage", operationResult.Result);
            }
            return View("404");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditablePageUpdateViewModel page) {
            if (ModelState.IsValid) {
                var updateResult = _editablePageRepository.Update(page);
                if (updateResult.Ok) {
                    var friendlyPage = FriendlyUrlProvider.Default.GetPageByFriendlyId(page.Id);
                    if (friendlyPage == null) {
                        return View("ViewEditablePage", updateResult.Result);
                    }
                    return RedirectPermanent(string.Concat("/", friendlyPage.Permalink));
                }
            }
            ModelState.AddModelError("", "Page model is not valid");
            return View("EditEditablePage", page);
        }

        #endregion

        #region helpers for 404 and exception
        public ActionResult General(Exception exception) {
            return View("Exception", exception);
        }

        public ActionResult Http404() {
            return View("404");
        }
        #endregion
    }
}
