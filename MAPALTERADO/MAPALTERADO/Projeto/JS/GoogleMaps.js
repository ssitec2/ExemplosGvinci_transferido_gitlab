function codeAddress(MapName, MapType, Address, ZoomLevel)
{
	var GoogleMapsGeocoder = new google.maps.Geocoder();
	GoogleMapsGeocoder.geocode({ 'address': Address }, function (results, status) 
	{
		if (status == google.maps.GeocoderStatus.OK) 
		{
			var myOptions =
			{
				zoom: ZoomLevel,
				center: results[0].geometry.location,
				mapTypeId: MapType
			}
			var GoogleMap = new google.maps.Map(document.getElementById(MapName), myOptions);
			var marker = new google.maps.Marker
			({
				map: GoogleMap,
				position: results[0].geometry.location
			});
			var infowindow = new google.maps.InfoWindow
			({ 
				content: results[0].formatted_address,        
				position: results[0].geometry.location    
			});
			infowindow.open(GoogleMap);
		}
		else 
		{
			alert("Erro ao procurar lugar! Status: " + status);
		}
	});
}
