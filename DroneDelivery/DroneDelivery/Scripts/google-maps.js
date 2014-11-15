function DroneMap(el)
{
    this.mapOptions = {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8
    };

    this.id = $(el).attr('id');

    $(el).html('<div class="gmap"></div>');

    this.map = new google.maps.Map($(el).find('.gmap')[0], this.mapOptions);
}

$(function () {
    var dronemaps = [];

    $('.dronemap').each(function (i, e) {
        dronemaps.push(new DroneMap(el));
    });

});