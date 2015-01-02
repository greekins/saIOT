function formatEventData(item) {
    return '<tr>' + '<td>' + item.TenantId + '</td>' + '<td>' + item.Name + '</td>' + '<td>' + item.Timestamp + '</td>' + '<td>' + item.Type + '</td>' + '<td>' + item.Sender + '</td>' + '<td>' + item.Body + '</td>' + '</tr>';
}

function putConfig(actorId) {
    var mode = document.getElementById("on").checked == true ? "on" : "off";

    var config = {
        level: document.getElementById("level").value,
        radius: document.getElementById("radius").value,
        threshold: document.getElementById("threshold").value,
        interval: document.getElementById("interval").value,
        mode: mode
    }
    var data = {
        Location: document.getElementById("position").value,
        Type: document.getElementById("type").value,
        Config: JSON.stringify(config)
    }

    $.ajax({
        url: "/api/actors/" + viewbagTenant + "/config/" + actorId,
        type: 'PUT',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function () { location.reload(true); },
        error: function (result) {
            $('#commandError').append(
                '<div class="alert alert-danger">' +
                '<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>' +
                '<span class="sr-only">Error: </span>' + result.status + ' ' + result.statusText +
            '</div>');
        },
    });
}


function getEvents() {
    $.getJSON("/api/events/" + viewbagTenant)
        .done(function (data) {
            $.each(data, function () {
                $('#entry').append(formatEventData(this)).data(data);
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#eventsError').append(
                '<div class="alert alert-danger">' +
                '<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>' +
                '<span class="sr-only">Error: </span>' + ' ' + jqXHR.status + ' ' + err +
                '</div>');
        });
}
function getActors() {
    $.getJSON("/api/actors/" + viewbagTenant + "/list/")
        .done(function (data) {
            $.each(data, function (i, item) {
                $('#dropdown').append(formatDropdown(this));
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#allEntries').text('Error: ' + err);
        });
}
function formatDropdown(item) {
    return '<option onclick="selectionChanged()" value="' + item.ActorId + '">' + item.Name + " - " + item.Location + "</option>";
}


function getActorDetails(id) {
    $.getJSON("/api/actors/" + viewbagTenant + "/details/" + id)
        .done(function (data) {
            fillForm(data);
        });
}

function selectionChanged() {
    var e = document.getElementById("dropdown");
    var id = e.options[e.selectedIndex].value;
    $('#commandError').hide();
    getActorDetails(id);
}

function fillForm(object) {

    var config = object.Config;
    var position = document.getElementById("position");

    position.value = object.Location;
    var type = document.getElementById("type");
    type.value = object.Type;
    var level = document.getElementById("level");
    level.value = config.level;
    var radius = document.getElementById("radius");
    radius.value = config.radius;
    var threshold = document.getElementById("threshold");
    threshold.value = config.threshold;
    var interval = document.getElementById("interval");
    interval.value = config.interval;

    if (config.mode == "on") {
        var on = document.getElementById("on");
        on.checked = true;
    }
    if (config.mode == "off") {
        var off = document.getElementById("off");
        off.checked = true;
    }
}