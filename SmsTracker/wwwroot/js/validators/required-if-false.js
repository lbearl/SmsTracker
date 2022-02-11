(function ($) {

    var $jQval = $.validator;

    $jQval.addMethod("requirediffalse",
        function (value, element, parameters) {
            return value === "" || value == null;
        }
    );

    var adapters = $jQval.unobtrusive.adapters;
    adapters.addBool('requirediffalse');

})(jQuery);