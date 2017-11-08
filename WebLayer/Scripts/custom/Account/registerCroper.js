var imageType;

var cropperLoadedImage = true;
var enableCropEvent = true;

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
            //if (enableCropEvent)
            //    setMinimalCropBoxSize();
            //Події кропера будуть доступні
            enableCropEvent = true;
        },
    };



    //Адреса зображення оригінального
    var originalImageURL = $image.attr('src');

    var uploadedImageURL; 

    $image.on({
    }).cropper(options);

    // Import image
    //Коли загрузив фотку
    $(document).on('change', '.inputImage', function () {
        console.log("Info");
        //Зображення було завтажено натиснути на кнопку Load type="file"
        cropperLoadedImage = true;
        //Кропер може реагувати а події 
        enableCropEvent = true;

        //Список файлів, які обрали
        var files = this.files;
        //Перший файл із списку файлів
        var file;

        //Якщо зображення не включає атребут 'cropper', тодів виходимо
        //if (!$image.data('cropper')) {
        //    return;
        //}

        //Якщо файл обрано
        if (files && files.length) {
            file = files[0]; //Беремо перший файл із списку
            imageType = file.type; //ти файлу
            console.log(imageType);
            //Якщо тип зображення
            if (/^image\/\w+$/.test(file.type)) {
                //Якщо силка на файл була збережена, то її видаляємо
                if (uploadedImageURL) {
                    URL.revokeObjectURL(uploadedImageURL);
                }
                //Зберігаємо силку на файл (записуємо як статичний)
                uploadedImageURL = URL.createObjectURL(file);
                console.log(uploadedImageURL);


                $('.inputImage').val('');
            } else {
                //Видаємо повідомлення, що потрібно обрати зображення
                window.alert('Please choose an image file.');
            }
            //У елемент зобржаення зписуємо адресу зобржаження яке ми зробили на основі нашого URL
            $image.cropper('destroy').attr('src', uploadedImageURL).cropper(options);
        }

    });
});