// Define the SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/QuizHub") // Specify the URL of your SignalR hub
    .build();

// Start the SignalR connection
connection.start()
    .then(function () {
        console.log("SignalR connected.");
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

// Export the connection variable so it can be used in other JavaScript files
