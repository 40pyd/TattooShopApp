$(document).ready(function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    connectin.on("SendMessage", (user, message) => {
        const msg = message.replace(/&/g, "&amp").replace(/</g, "&alt").replace(/>/g, "&gt;");
        const encoding = user + ": " + message;
        var li = document.createElement("li");
        li.textContent = encoding;
        document.getElementById("messageList").appendChild(li);
    });

    connection.start().catch(error => console.log(error.toString()));

    document.getElementById("chatSendButton").addEventListener("click", event => {
        const user = document.getElementById("userInput").value;
        const message = document.getElementById("messageInput").value;

        connection.invoke("SendMessage", user, message).catch(error => console.log(error.toString()));
        event.preventDefault();
    });



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

//function openForm() {
//    document.getElementById("chat").style.display = "block";
//}

//function closeForm() {
//    document.getElementById("chat").style.display = "none";
//}