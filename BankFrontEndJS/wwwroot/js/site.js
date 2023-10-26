// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function loadView(status) {
    var apiUrl = '/login/defaultview';
    if (status === "authview")
        apiUrl = '/login/authview';
    if (status === "error")
        apiUrl = '/login/error';
    if (status === "about")
        apiUrl = '/about/view';
    if (status === "logout")
        apiUrl = '/logout';
    if (status === "account")
        apiUrl = '/account';
    if (status === "transfer")
        apiUrl = '/transfer';
    if (status === "profile")
        apiUrl = '/profile';

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            document.getElementById('main').innerHTML = data;
            if (status === "logout") {
                document.getElementById('LogoutButton').style.display = "none";
                document.getElementById('ProfileButton').style.display = "none";
                document.getElementById('AccountButton').style.display = "none";
                document.getElementById('TransactionsButton').style.display = "none";
                document.getElementById('TransferButton').style.display = "none";
            } else if (status == "profile") {

                document.getElementById('ok-btn-name').style.display = "none";
                document.getElementById('ok-btn-email').style.display = "none";
                document.getElementById('ok-btn-number').style.display = "none";

                document.getElementById('name-change-text').style.display = "none";
                document.getElementById('email-change-text').style.display = "none";
                document.getElementById('number-change-text').style.display = "none";

            } else if (status == "account") {

                fetch('/gettransactions')
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(data => {
                        const container = document.getElementById('transactionsContainer');

                        for (let i = 0; i < data.length; i += 4) {
                            const transactionType = data[i];
                            const transactionAmount = data[i + 1];
                            const transactionTimestamp = data[i + 2];
                            const transactionAccount = data[i + 3];

                            const transactionDiv = document.createElement('div');
                            transactionDiv.className = 'transaction';

                            const typeDiv = document.createElement('div');
                            typeDiv.className = 'transaction-type';
                            typeDiv.innerHTML = `Type: <span>${transactionType}</span>`;

                            const amountDiv = document.createElement('div');
                            amountDiv.className = 'transaction-amount';
                            amountDiv.innerHTML = `Amount: <span>$${transactionAmount}</span>`;

                            const timestampDiv = document.createElement('div');
                            timestampDiv.className = 'transaction-timestamp';
                            timestampDiv.innerHTML = `Date & time: <span>${transactionTimestamp}</span>`;

                            const accountNo = document.createElement('div');
                            accountNo.className = 'transaction-accountNo';
                            accountNo.innerHTML = `Account: <span>${transactionAccount}</span>`;

                            transactionDiv.appendChild(typeDiv);
                            transactionDiv.appendChild(amountDiv);
                            transactionDiv.appendChild(timestampDiv);
                            transactionDiv.appendChild(accountNo);

                            container.appendChild(transactionDiv);
                        }
                    })
                    .catch(error => {
                        console.log('There was a problem with the fetch operation:', error.message);
                    });
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}


function performAuth() {

    var name = document.getElementById('SName').value;
    var password = document.getElementById('SPass').value;
    var data = {
        UserName: name,
        PassWord: password
    };
    console.error(data);
    const apiUrl = '/login/auth';

    const headers = {
        'Content-Type': 'application/json',
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data)
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview");
                document.getElementById('LogoutButton').style.display = "block";
                document.getElementById('ProfileButton').style.display = "block";
                document.getElementById('AccountButton').style.display = "block";
                document.getElementById('TransactionsButton').style.display = "block";
                document.getElementById('TransferButton').style.display = "block";

            }
            else {
                loadView("error");
            }

        })
        .catch(error => {
            console.error('Fetch error:', error);
        });    
}

function toggleEdit(id) {
    const inputBox = document.getElementById(id + '-change-text').style.display = "block";
    const okButton = document.getElementById('ok-btn-' + id).style.display = "block";
}

function confirmEdit(id) {
    var edit = document.getElementById(id + '-change-text').value;

    const inputBox = document.getElementById(id + '-change-text').style.display = "none";
    const okButton = document.getElementById('ok-btn-' + id).style.display = "none";

    // postfix so that profile controller can identify what has been changed
    edit += '+' + id;

    var data = {
        UserName: edit,
        PassWord: ""
    };
    const apiUrl = '/profile';

    const headers = {
        'Content-Type': 'application/json',
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) 
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .catch(error => {            
            console.error('Fetch error:', error);
        });

    loadView('profile');
}

function submitTransfer() {
    var fromAccount = document.getElementById("FromAccount").value;
    var toAccount = document.getElementById("ToAccount").value;
    var amount = document.getElementById("Amount").value;
    var description = document.getElementById("Remarks").value;

    if (fromAccount === toAccount) {
        alert("You cannot transfer money to the same account");
        return false;  
    }

    if (parseFloat(amount) <= 0) {
        alert("Please enter a valid transfer amount greater than 0");
        return false;  
    }

    // need to create 2 new transactions, one for sender (from) and one for reciever (to)
    var intFromAccount = parseInt(fromAccount, 10);
    var intToAccount = parseInt(toAccount, 10);
    //let sendAccNum = 

    var transactionFrom = {
        TransactionId: 9999,
        AccountNumber: intFromAccount,
        Type: 1, // 0 is deposit 1 is withdraw
        // definitely need exception handling for the float XDD
        Amount: parseFloat(amount),
        Timestamp: new Date(), // sets the default to the current date and time
        Account: {}
    };

    var transactionTo = {
        TransactionId: 9999,
        AccountNumber: intToAccount,
        Type: 0,
        Amount: parseFloat(amount),
        Timestamp: new Date(), 
        Account: {} 
    };

    //let transactionList = [transactionFrom, transactionTo];

    const apiUrl = '/transactions';

    const headers = {
        'Content-Type': 'application/json',
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(transactionFrom)
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });

    const requestOptions1 = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(transactionTo)
    };

    fetch(apiUrl, requestOptions1)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });

}

/*function submitTransfer() {

}*/



/*function updateContactInfo() {

}*/


document.addEventListener("DOMContentLoaded", loadView);
/*
const loginButton = document.getElementById('LoginButton');
loginButton.addEventListener('click', loadView);

const aboutButton = document.getElementById('AboutButton');
aboutButton.addEventListener('click', loadView("about"));

const logoutButton = document.getElementById('LogoutButton');
logoutButton.addEventListener('click', loadView("logout"));*/




