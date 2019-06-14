$(document).ready(function () {
    var clickCnt = 0;

    function getPersonAsJson() {
        clickCnt++;
        var $form = $(this).closest('form');
        var data = $form.serialize();
        $('#loading-indicator').show();
        $('#nextBtn').prop("disabled", true);
        $('#prevBtn').prop("disabled", true);
        $("#PremiumAzn").val("");
        $.ajax({
            type: 'POST',
            url: '/az/Insurance/CalculatePremiumAsync',
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: data,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                console.log('Data received: ');
                console.log(result);
                clickCnt--;
                if (clickCnt === 0) {
                    $('#loading-indicator').hide();
                    $("#PremiumAzn").val(result);
                    if (result > 0) {
                        $('#nextBtn').prop("disabled", false);
                        $('#prevBtn').prop("disabled", false);
                    }
                }
            },
            error: function (xhr, status, error) {
                conso.log('Error occured');
                $('#nextBtn').prop("disabled", true);
                $('#prevBtn').prop("disabled", true);
                $('#loading-indicator').hide();
                alert(error);
            }
        });
    }

    $("#EndDate").on('change', getPersonAsJson);
    $("#StartDate").on('change', getPersonAsJson);
    $("#Currency").on('change', getPersonAsJson);
    $("#GuaranteeTimeId").on('change', getPersonAsJson);
    $("#GroupInsuredId").on('change', getPersonAsJson);
    $("#InsuranceAreaId").on('change', getPersonAsJson);
    $("#InsuranceZoneId").on('change', getPersonAsJson);
    $("#SumOfInsuredsId").on('change', getPersonAsJson);

    var inputPassportFin = document.getElementById('passport_fin_input');
    var inputPassportNumb = document.getElementById('passport_number_input');
    var inputPassportVisa = document.getElementById('passport_visa_input');

    var imgPassportFin = document.getElementById('passport_fin');
    var imgPassportNumber = document.getElementById('passport_number');
    var imgPassportVisa = document.getElementById('passport_visa');

    if (inputPassportFin !== null) {
        inputPassportFin.addEventListener('focus', function () {
            imgPassportFin.style.display = 'block';
        });
        inputPassportFin.addEventListener('focusout', function () {
            imgPassportFin.style.display = 'none';
        });

        inputPassportNumb.addEventListener('focus', function () {
            imgPassportNumber.style.display = 'block';
        });
        inputPassportNumb.addEventListener('focusout', function () {
            imgPassportNumber.style.display = 'none';
        });

        inputPassportVisa.addEventListener('focus', function () {
            imgPassportVisa.style.display = 'block';
        });
        inputPassportVisa.addEventListener('focusout', function () {
            imgPassportVisa.style.display = 'none';
        });
    }
});