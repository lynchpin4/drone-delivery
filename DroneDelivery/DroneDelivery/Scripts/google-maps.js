// Quick n' Dirty
var DEFAULT_LAT = 47.622279;
var DEFAULT_LNG = -122.336829;

// for order page
var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};

function DroneMap(el)
{
    console.log('initializing drone map: ' + el);

    this.mapOptions = {
        center: { lat: DEFAULT_LAT, lng: DEFAULT_LNG },
        zoom: 20,
        streetViewControl: false,
        mapTypeId: google.maps.MapTypeId.HYBRID
    };

    this.id = $(el).attr('id');
    this.el = $(el);

    this.el.html('<input class="controls pac-input" type="text" placeholder="Start typing address here.." /><div class="gmap"></div>');

    var map = this.map = new google.maps.Map($(el).find('.gmap')[0], this.mapOptions);

    // Optional Stuff
    if ($(el).is('[search]'))
        this.addSearchBox();

    if ($(el).is('[nozoom]'))
        this.limitZoomLevel();

    if ($(el).is('[defaultloc]'))
    {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                initialLocation = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                map.setCenter(initialLocation);
                map.setZoom(20);
                this.setApiPosition(initialLocation);
            }.bind(this));
        }
    }

    this.marker = new google.maps.Marker({
        map: map,
        draggable: true,
        position: new google.maps.LatLng(DEFAULT_LAT, DEFAULT_LNG)
    });

    google.maps.event.addListener(this.marker, 'dragend', function () {
        this.setApiPosition(this.marker.getPosition());
    }.bind(this));
}

DroneMap.prototype.setApiPosition = function(position)
{
    if (this.locationChanged)
    {
        this.locationChanged(position);
    }
    else
    {
        $('#latitude').val(position.lat());
        $('#longitude').val(position.lng());
    }

    console.log('drone map position updated: ');
    console.dir(position);
    window.map_position = position;
}

DroneMap.prototype.limitZoomLevel = function()
{
    var map = this.map;

    // Limit the zoom level
    google.maps.event.addListener(this.map, 'zoom_changed', function () {
        if (map.getZoom() < this.minZoomLevel) map.setZoom(this.minZoomLevel);
    });
}

DroneMap.prototype.addSearchBox = function()
{
    var input = this.el.find('.pac-input')[0];
    this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

   // var searchBox = new google.maps.places.SearchBox(input);

    var autocomplete = this.autocomplete = new google.maps.places.Autocomplete(input, { types: ['geocode'] });
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        this.fillInAddress();
    }.bind(this));
}

DroneMap.prototype.fillInAddress = function() {
    // Get the place details from the autocomplete object.
    var place = this.autocomplete.getPlace();
    this.marker.setPosition(place.geometry.location);
    this.setApiPosition(place.geometry.location);

    for (var component in componentForm) {
        document.getElementById(component).value = '';
        document.getElementById(component).disabled = false;
    }

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            document.getElementById(addressType).value = val;
        }
    }
}

// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
DroneMap.prototype.geolocate = function() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
                position.coords.latitude, position.coords.longitude);

            this.marker.setPosition(geolocation);
            this.setApiPosition(geolocation);

            autocomplete.setBounds(new google.maps.LatLngBounds(geolocation,
                geolocation));
        }.bind(this));
    }
}

$(function () {
    var dronemaps = [];

    $('.dronemap').each(function (i, e) {
        try
        {
            window.drone_map = new DroneMap(e);
            dronemaps.push(drone_map);
        } catch (ex) {
            console.dir(ex);
        }
    });
    
});