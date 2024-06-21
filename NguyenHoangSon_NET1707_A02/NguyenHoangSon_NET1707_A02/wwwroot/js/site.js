$(() => {
    LoadProdData();
    var connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Debug)
        .withUrl("/signalRServer", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();
    connection.start().then(function () {
        console.log('Connected!');
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("LoadProducts", function () {
        LoadProdData();
    })

    LoadProdData();

    function LoadProdData() {
        var tr = '';
        $.ajax({
            url: '/RoomInformations/?handler=GetData',
            dataType: 'json',
            method: 'GET',
            success: (result) => {
                console.log(result);
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.RoomId} </td> 
                        <td> ${v.RoomNumber} </td> 
                        <td> ${v.RoomDetailDescription} </td>
                        <td> ${v.RoomMaxCapacity} </td>
                        <td> ${v.RoomStatus} </td>
                        <td> ${v.RoomPricePerDay} </td>
                        <td>
                            <a href='../Products/Edit?id=${v.RoomId}'> Edit </a> | 
                            <a href='../Products/Details?id=${v.RoomId}'> Details </a> | 
                            <a href='../Products/Delete?id=${v.RoomId}'> Delete </a>
                        </td>
                        </tr>`
                })
                console.log(tr);
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.error("AJAX Error: ", error);
                console.error("Status: ", error.status);
                console.error("Status Text: ", error.statusText);
                console.error("Response Text: ", error.responseText);
            }

        });
    }
}

)