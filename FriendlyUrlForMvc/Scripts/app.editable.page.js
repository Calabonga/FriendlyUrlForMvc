var Site = Site || {};

Site.models = Site.models || {};

$(function () {
    $(".froala-editor").froalaEditor({
        language: 'ru',
        heightMax: 500,
        htmlAllowedAttrs: ['title', 'href', 'alt', 'src'],
        htmlAllowedTags: ['p', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'img']
    });
});