$(() => {
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
    });

    LoadProdData();

    function LoadProdData() {
        var pageNumber = $('#pageNumber').val();
        var tr = '';
        $.ajax({
            url: '/RoomInformations/?handler=SignalR&pageNumber=' + pageNumber,
            dataType: 'json',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    var statusText = v.RoomStatus == 1 ? 'Active' : 'Deleted';
                    tr += `<tr>
                        <td> ${v.RoomNumber} </td> 
                        <td> ${v.RoomDetailDescription} </td>
                        <td> ${v.RoomMaxCapacity} </td>
                        <td><span>${statusText}</span></td>
                        <td> ${v.RoomPricePerDay} </td>
                        <td> ${v.RoomType.RoomTypeName} </td>
                        <td style="width: 20%">
                            <div class="btn-group" role="group" aria-label="Basic example">
                                <form class="btn-group"  action="/RoomInformations/Edit" method="get" >
                                    <input type="hidden" name="id" value="${v.RoomId}" />
                                    <button type="submit" class="btn btn-warning">Edit</button>
                                </form>
                                <form class="btn-group" action="/RoomInformations/Details" method="get" >
                                    <input type="hidden" name="id" value="${v.RoomId}" />
                                    <button type="submit" class="btn btn-success">Detail</button>
                                </form>
                                <form class="btn-group" action="/RoomInformations/Delete" method="get" >
                                    <input type="hidden" name="id" value="${v.RoomId}" />
                                    <button type="submit" class="btn btn-danger">Delete</button>
                                </form>
                            </div>
                        </td>
                    </tr>`;
                });
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
});
