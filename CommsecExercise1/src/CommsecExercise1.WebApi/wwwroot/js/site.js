var apiPath = "";         //the path for api aervice methods should be something like "http://localhost:62010/api/";

$(document).ready(function () {
    apiPath = "http://" + window.location.host + "/api/";
});

function executeTestMethod(btn) {
    resultPanel.value = "";
    var httpMethod = btn.value;
    var tr = btn.parentElement.parentElement;
    var tdApiMethod = tr.children[0];
    var tdApiParam = tr.children[1];

    var apiMethod = tdApiMethod.innerHTML;                 // apiTd.childNodes[0].childNodes[0].innerHTML;
    var apiParam = tdApiParam.children[0].value;           // children >> html collection

    if (apiParam.toString().startsWith("/")) {             // then the Get sends params as part of the route not as part of the query string 
        apiMethod = apiMethod + apiParam;
        apiParam = "";
    }


    $.ajax({
        url: apiPath + apiMethod,
        type: httpMethod,
        data: apiParam,
        datatype: "json",                                  //type of data accepted "accept" 
        contentType: "application/json; charset=utf-8",    //type of data sent

        success: function (data, status, xhr) {

            var s = JSON.stringify(data, null, 3);
            if (typeof data === 'undefined') {
                resultPanel.value = 'no content returned.';
            } else {
                resultPanel.value = s;
            }
        },

        error: function (xhr, status, err) {

            var s = "status:" + xhr.status + "\n";
            s = s + "statusText:" + xhr.statusText + "\n";
            s = s + "responseText:" + xhr.responseText;
            resultPanel.value = s;
        }
    });
}
