$(document).ready(function () {
    GetMap();
});

// Функция загрузки
function GetMap() {

    google.maps.visualRefresh = true;
    var ScullTattooStudio = new google.maps.LatLng(50.442998, 30.495093);

    var mapOptions = {
        zoom: 15,
        center: ScullTattooStudio,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    };

    var map = new google.maps.Map(document.getElementById("canvas"), mapOptions);
    var myLatlng = new google.maps.LatLng(50.442998, 30.495093);

    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: '2ScullTattooStudioShop'
    });

    marker.setIcon('https://maps.google.com/mapfiles/ms/icons/red-dot.png')
}