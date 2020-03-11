﻿Sys.Application.add_load(function () {
    /* Threestep gateway related */
    var $step2Submit = $('.js-step2-submit');

    // {{validationGroup}} will get replaced with whatever the validationGroup is
    var validationGroup = '{{validationGroup}}';
    var $step2Url = $('.js-step2-url');
    var $updateProgress = $('#updateProgress');
    var $iframeStep2 = $('.js-step2-iframe');
    var $addressControl = $('.js-billingaddress-control');

    // {{postbackControlReference}} will get replaced with whatever the postback control reference is
    var postbackControlReferenceScript = "javascript:{{postbackControlReference}}";

    // Posts the iframe (step 2)
    $step2Submit.on('click', function (e) {
        e.preventDefault();

        var $submitButton = $(this);
        if ($submitButton.attr('disabled')) {
            return;
        }

        debugger

        var paymentType = $('.js-payment-tab').val() ?? 'CreditCard';

        if (typeof (Page_ClientValidate) == 'function') {
            if (Page_IsValid && Page_ClientValidate(validationGroup)) {
                $submitButton.attr('disabled', 'disabled');

                var src = $step2Url.val();
                var $form = $iframeStep2.contents().find('#Step2Form');
                var $cbBillingAddressCheckbox = $('.js-billing-address-checkbox');
                var populateBilling = true;
                if ($cbBillingAddressCheckbox.length == 0) {
                    populateBilling = true;
                } else {
                    populateBilling = ($cbBillingAddressCheckbox.is(':visible') && ($cbBillingAddressCheckbox.prop('checked')));
                }

                if (populateBilling) {
                    $form.find('.js-billing-address1').val($('.js-street1', $addressControl).val());
                    $form.find('.js-billing-city').val($('.js-city', $addressControl).val());
                    $form.find('.js-billing-state').val($('.js-state', $addressControl).val());
                    $form.find('.js-billing-postal').val($('.js-postal-code', $addressControl).val());
                    $form.find('.js-billing-country').val($('.js-country', $addressControl).val());
                }
            }

            if (paymentType == 'CreditCard') {
                var $cardFirstName = $('.js-creditcard-firstname');
                var $cardLastName = $('.js-creditcard-lastname');
                var $cardFullName = $('.js-creditcard-fullname');
                var $cardNumber = $('.js-creditcard-number');
                var $cardExpMonth = $('.js-monthyear-month');
                var $cardExpYear = $('.js-monthyear-year');
                var $cardCVV = $('.js-creditcard-cvv');
                var validationMessage = '';
                if ($cardNumber.val() == '') {
                    validationMessage += '<li>' + 'Card number is required' + '</li>'
                }

                if ($cardExpMonth.val() == '' || $cardExpYear.val() == '') {
                    validationMessage += '<li>' + 'Expiration is required' + '</li>'
                }

                if ($cardCVV.val() == '') {
                    validationMessage += '<li>' + 'CVV is required' + '</li>'
                }

                if (validationMessage != '') {
                    var $creditCardValidationNotification = $('.js-creditcard-validation-notification');
                    $('.js-notification-text', $creditCardValidationNotification).html('<ul>' + validationMessage + '</ul>');
                    $creditCardValidationNotification.show();
                    $submitButton.removeAttr('disabled');
                    return false;
                }

                $form.find('.js-cc-first-name').val($cardFirstName.val());
                $form.find('.js-cc-last-name').val($cardLastName.val());
                $form.find('.js-cc-full-name').val($cardFullName.val());
                $form.find('.js-cc-number').val($cardNumber.val());
                var mm = $cardExpMonth.val();
                var yy = $cardExpYear.val();
                mm = mm.length == 1 ? '0' + mm : mm;
                yy = yy.length == 4 ? yy.substring(2, 4) : yy;
                $form.find('.js-cc-expiration').val(mm + yy);
                $form.find('.js-cc-cvv').val($cardCVV.val());
            } else {
                var $achAccountName = $('.js-ach-accountname');
                var $achAccountNumber = $('.js-ach-accountnumber');
                var $achAccountRoutingNumber = $('.js-ach-routingnumber');
                var $achAccountType = $('.js-ach-accounttype').find('input:checked');

                var validationMessage = '';
                if ($achAccountName.val() == '') {
                    validationMessage += '<li>' + 'Account name is required' + '</li>'
                }
                if ($achAccountNumber.val() == '') {
                    validationMessage += '<li>' + 'Account number is required' + '</li>'
                }
                if ($achAccountRoutingNumber.val() == '') {
                    validationMessage += '<li>' + 'Routing number is required' + '</li>'
                }
                if (validationMessage != '') {
                    var $achValidationNotification = $('.js-ach-validation-notification');
                    $('.js-notification-text', $achValidationNotification).html('<ul>' + validationMessage + '</ul>');
                    $achValidationNotification.show();
                    $submitButton.removeAttr('disabled');
                    return false;
                }

                $form.find('.js-account-name').val($achAccountName.val());
                $form.find('.js-account-number').val($achAccountNumber.val());
                $form.find('.js-routing-number').val($achAccountRoutingNumber.val());
                $form.find('.js-account-type').val($achAccountType.val());
                $form.find('.js-entity-type').val('personal');
            }

            $updateProgress.show();
            $form.attr('action', src);
            $form.submit();
        }
    });

    // Evaluates the current url whenever the iframe is loaded and if it includes a qrystring parameter
    // The qry parameter value is saved to a hidden field and a post back is performed
    $('#iframeStep2').on('load', function (e) {
        try {
            var location = this.contentWindow.location;
            var qryString = this.contentWindow.location.search;
            if (qryString && qryString != '' && qryString.startsWith('?token-id')) {
                $('.js-step2-returnquerystring').val(qryString);
                window.location = postbackControlReferenceScript;
            } else {
                if ($('.js-step2-autosubmit').val() == 'true') {
                    $('#updateProgress').show();
                    var src = $('.js-step2-url').val();
                    var $form = $('#iframeStep2').contents().find('#Step2Form');
                    $form.attr('action', src);
                    $form.submit();
                    $('#updateProgress').hide();
                }
            }
        }
        catch (err) {
            debugger
            $('#updateProgress').hide();
            console.log(err);
        }
    });
});
