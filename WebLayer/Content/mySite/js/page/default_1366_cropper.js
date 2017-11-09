var imageType;

var cropperLoadedImage = true;
var enableCropEvent = true;

//Для поререднього прегляду на сайті
function displayImagePreview(result) {
    var div = $('.background-image-preview-wrapper');
    div.removeClass('empty-image');
    div.removeClass('pointer');
    div.css('background-image', "url('" + result.toDataURL(imageType) + "')");
    if (typeof(preview_foto_bg) !== "undefined")
    {
        var divPrew = $('.foto_bg');
        divPrew.css('background-image', "url('" + result.toDataURL(imageType) + "')");
    }

}

$(function () {

    'use strict';


    var console = window.console || { log: function () { } };
    //Url сторінки
    var URL = window.URL || window.webkitURL;
    //Зображення, яке будемо кропати
    var $image = $('#image');
    //Налаштування кропера
    var options = {
        //Обмежуємо область обрізки, щоб він не виходив за межі хоста
        viewMode: 1,
        //Число от 0 до 1. Определите размер области автоматической обрезки (в процентах)
        autoCropArea: 0.00000001,
        //Співідношення сторін
        aspectRatio: 1.5,
        //Фукнція викликається, тоді коли кропер починає кропати зображення
        crop: function () {
            //Якщо тільки завантажили зображення для кропера
            if (cropperLoadedImage) {
                //console.log("crop image");
                //Отримуємо канвас дата
                //naturalWidth: естественная ширина холста (только для чтения)
                //naturalHeight: естественная высота холста (только для чтения)
                //width: ширина холста
                //height: высота холста
                var canvasData = $image.cropper('getCanvasData');
                if (canvasData.naturalWidth < canvasData.width) {
                    console.log("enableCropEvent=false");
                    enableCropEvent = false;
                    //Увеличьте размер холста (обертка изображения) до абсолютного соотношения.
                    $image.cropper('zoomTo', 1);
                    //Виставляємо мінімально можливий розмір блока по виделенню
                    setMinimalCropBoxSize();
                }
                //Ставимо флажок, що зображення уже звантажили
                cropperLoadedImage = false;
            }
            //Виставляємо мінімально можливий розмір блока по виделенню
            if (enableCropEvent)
                setMinimalCropBoxSize();
            //Події кропера будуть доступні
            enableCropEvent = true;
        },
    };

    //Виставляємо мінімально можливий розмір блока по виделенню
    var setMinimalCropBoxSize = function () {
        //Область виділення на коропері
        var data = $image.cropper('getData');
        if (data.width < 400) {
            //Якщо ширина менше вказаної, тоді відключаємо події
            enableCropEvent = false;
            $image.cropper('setData', { width: 400, height: 300 });
        }
    };

    //Адреса зображення оригінального
    var originalImageURL = $image.attr('src');

    var uploadedImageURL; 


    $('[data-toggle="tooltip"]').tooltip();

    $image.on({
    }).cropper(options);


    // Buttons
    if (!$.isFunction(document.createElement('canvas').getContext)) {
        $('button[data-method="getCroppedCanvas"]').prop('disabled', true);
    }

    if (typeof document.createElement('cropper').style.transition === 'undefined') {
        $('button[data-method="rotate"]').prop('disabled', true);
        $('button[data-method="scale"]').prop('disabled', true);
    }

    // Options
    $('.docs-toggles').on('change', 'input', function () {
        var $this = $(this);
        var name = $this.attr('name');
        var type = $this.prop('type');
        var cropBoxData;
        var canvasData;

        if (!$image.data('cropper')) {
            return;
        }

        if (type === 'checkbox') {
            options[name] = $this.prop('checked');
            cropBoxData = $image.cropper('getCropBoxData');
            canvasData = $image.cropper('getCanvasData');

            options.built = function () {
                $image.cropper('setCropBoxData', cropBoxData);
                $image.cropper('setCanvasData', canvasData);
            };
        } else if (type === 'radio') {
            options[name] = $this.val();
        }

        $image.cropper('destroy').cropper(options);
    });


    // Methods
    $('.docs-buttons').on('click', '[data-method]', function () {
        var $this = $(this);
        var data = $this.data();
        var $target;
        var result;

        if ($this.prop('disabled') || $this.hasClass('disabled')) {
            return;
        }

        if ($image.data('cropper') && data.method) {
            data = $.extend({}, data); // Clone a new one

            if (typeof data.target !== 'undefined') {
                $target = $(data.target);

                if (typeof data.option === 'undefined') {
                    try {
                        data.option = JSON.parse($target.val());
                    } catch (e) {
                        console.log(e.message);
                    }
                }
            }

            if (data.method === 'rotate') {
                $image.cropper('clear');
            }
            //Отримуємо дані з кропера
            result = $image.cropper(data.method, data.option, data.secondOption);

            if (data.method === 'rotate') {
                $image.cropper('crop');
            }

            switch (data.method) {
                case 'closeEditor':
                    $("#cropper").css("display", "none");
                    $image.cropper('destroy').cropper(options);
                    break;
                case 'scaleX':
                case 'scaleY':
                    $(this).data('option', -data.option);
                    break;
                    //Це так кнопка коли нажав ок в кропері
                case 'getCroppedCanvas':
                    if (result) {

                        //Якщо дані з кропера менше заданої ширини, то видаємо повідомлення
                        if (result.width < 400) {
                            BootstrapDialog.show({
                                cssClass: 'modal-up',
                                title: 'Помилка',
                                message: 'Ширина зображення повинна бути не менше 400 px',
                                type: BootstrapDialog.TYPE_DANGER
                            });
                            return false;
                        }
                        //Показуємо дане зображення
                        displayImagePreview(result);
                        //Беремо скрите поле на формі
                        var hiddenInput = $('#Image')
                        //Кладемо в нього дані у форматі base64, при цьму вказуємо тип зображення
                        hiddenInput.val(result.toDataURL(imageType));
                        hiddenInput.keyup();
                        console.log("Image base 64");
                        console.log(result.toDataURL(imageType));
                        $image.cropper('destroy').cropper(options);
                        $("#cropper").css("display", "none");
                            
                    }

                    break;

                case 'destroy':
                    if (uploadedImageURL) {
                        URL.revokeObjectURL(uploadedImageURL);
                        uploadedImageURL = '';
                        $image.attr('src', originalImageURL);
                    }

                    break;
            }

            if ($.isPlainObject(result) && $target) {
                try {
                    $target.val(JSON.stringify(result));
                } catch (e) {
                    console.log(e.message);
                }
            }

        }
    });


    // Keyboard
    $(document.body).on('keydown', function (e) {

        if (!$image.data('cropper') || this.scrollTop > 300) {
            return;
        }

        switch (e.which) {
            case 37:
                e.preventDefault();
                $image.cropper('move', -1, 0);
                break;

            case 38:
                e.preventDefault();
                $image.cropper('move', 0, -1);
                break;

            case 39:
                e.preventDefault();
                $image.cropper('move', 1, 0);
                break;

            case 40:
                e.preventDefault();
                $image.cropper('move', 0, 1);
                break;
        }

    });


    // Import image
    //Коли загрузив фотку
    $(document).on('change', '.inputImage', function () {
        //Зображення було завтажено натиснути на кнопку Load type="file"
        cropperLoadedImage = true;
        //Кропер може реагувати а події 
        enableCropEvent = true;

        //Список файлів, які обрали
        var files = this.files;
        //Перший файл із списку файлів
        var file;
        //Якщо зображення не включає атребут 'cropper', тодів виходимо
        if (!$image.data('cropper')) {
            return;
        }
        //Якщо файл обрано
        if (files && files.length) {
            file = files[0]; //Беремо перший файл із списку
            imageType = file.type; //ти файлу
            console.log(file);
            //Якщо тип зображення
            if (/^image\/\w+$/.test(file.type)) {
                //Якщо силка на файл була збережена, то її видаляємо
                if (uploadedImageURL) {
                    URL.revokeObjectURL(uploadedImageURL);
                }
                //Зберігаємо силку на файл (записуємо як статичний)
                uploadedImageURL = URL.createObjectURL(file);


                $('.inputImage').val('');
            } else {
                //Видаємо повідомлення, що потрібно обрати зображення
                window.alert('Please choose an image file.');
            }
            console.log(uploadedImageURL);
            //У елемент зобржаення зписуємо адресу зобржаження яке ми зробили на основі нашого URL
            $image.cropper('destroy').attr('src', uploadedImageURL).cropper(options);
            //Показую блок кропера
            $("#cropper").css("display", "block");
        }

    });
    //Щоб по клікові по пустій превюю загружати фото 
    $(document).on('click', '.background-image-preview-wrapper[class*="empty-image"]', function () {
        $('.inputImage').click();
    });



    


});