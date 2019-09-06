<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebSocketJS.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.4.1.js"></script>
    <script src="Scripts/modernizr-2.8.3.js"></script>
        <script type="text/javascript">
        var socket;

        $(document).ready(function () {
            if (!Modernizr.websockets)
            {
                alert("This browser does not support web sockets");
            }

            socket = new WebSocket("ws://localhost:49464/WebSocketHandler.ashx");
            //socket = new WebSocket("ws://localhost:23000");
            socket.addEventListener("open", function (evt) {
                $("#divHistory").append('<h3>Connection Opened with the server.</h3>');
            }, false);

            socket.addEventListener("close", function (evt) {
                $("#divHistory").append('<h3>Connection was closed. :' + evt.reason + '</h3>');
            }, false);

            socket.addEventListener("message", function (evt) {
                //
                $("#divHistory").append('<h3>' + evt.data + '</h3>');
                //alert(evt.message);
            }, false);

            socket.addEventListener("error", function (evt) {
                //alert('Error : ' + evt.message);
                $("#divHistory").append('<h3>Error : ' + evt.message + '</h3>');
            }, false);

            $("#startButton").click(function () {
                if (socket.readyState == WebSocket.OPEN) {
                    var message;

                    if ($("#chbxBroadcasttoAll").prop('checked') === true) {
                        message = "b:" + $("#myEmail").val()
                    }
                    else {
                        message = $("#myEmail").val();
                    }
                    socket.send(message);
                }
                else {
                    $("#divHistory").append('<h3>The underlying connection is closed.</h3>');
                }
            });

            $("#endButton").click(function () {
                socket.close();
            });


        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <div>
            <input id="myEmail" type="text" placeholder="Add your message here." />
            <input id="startButton" type="button" autofocus="autofocus" value="Start" />
            <input id="endButton" type="button" value="End" /><br />
                <input id="chbxBroadcasttoAll" type="checkbox" /><label for="chbxBroadcasttoAll">Send to All</label>
&nbsp;<div id="divHistory"></div>
        </div>  
    </form>
</body>
</html>
