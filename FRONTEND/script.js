// Returns the numeric value corresponding to the WeatherType
// 0: Napos, 1: Borult, 2: Esos, 3: Havas
const weatherTypeIcons = [
  "sunny.png", // for Napos
  "cloudy.png",            // for Borult
  "rainy.png",  // for Esős
  "snowy.png"  // for Havas
];

function getEnumValue(selectedWeather) {
    switch (selectedWeather) {
      case "Napos": return 0;
      case "Borult": return 1;
      case "Esos": return 2;
      case "Havas": return 3;
      default: return 0;
    }
  }
  
  function getWeatherTypeName(enumValue) {
    switch (enumValue) {
      case 0: return "Napos";
      case 1: return "Borult";
      case 2: return "Esős";
      case 3: return "Havas";
      default: return 0;
    }
  }  
  
  // Global array to store weather data (for POST payload)
  let weatherDataArray = [];
  const dataList = document.getElementById("dataList");
  const resultDiv = document.getElementById("result");
  
  // NEW: grab the forecast panel and the previous‐list container
  const forecastContainer = document.getElementById("forecastContainer");
  const previousContainer = document.getElementById("previousContainer");

  // Event handler for "Adat hozzáadása" button: Convert selection to a numeric value and add to the array
  document.getElementById("addBtn").addEventListener("click", function(){
    const select = document.getElementById("weatherSelect");
    const selectedWeather = select.value; // "Napos", "Borult", "Esos", "Havas"
    // unshift instead of push to put it at the beginning of array
    weatherDataArray.unshift({ "Type": getEnumValue(selectedWeather) });
    updateDataList();
  });
  
  // Event handler for clearing the weatherDataArray and updating the display
  document.getElementById("clearBtn").addEventListener("click", function(){
    weatherDataArray = [];
    updateDataList();
  });
  
  // Updates the displayed list of stored weather data
  function updateDataList(){
    dataList.innerHTML = "";
    weatherDataArray.forEach((data, index) => {
      //let dayText = (index + 1) > 1 ? " Days ago" : " Day ago";
      let dayText = " napja";

      const li = document.createElement("li");
      li.className = "list-group-item";
      const text = document.createTextNode((index + 1) + dayText + ": " + getWeatherTypeName(data.Type) + " ");
      li.appendChild(text);
      
      // Create an image element for the pictogram
      const img = document.createElement("img");
      img.src = `images/${weatherTypeIcons[data.Type]}`;
      img.width = 32;  // Set width to 32px
      img.height = 32; // Set height to 32px
      img.style.verticalAlign = "middle";
      // Optional: add a small left margin for spacing
      img.style.marginLeft = "8px";
      
      li.appendChild(img);
      dataList.appendChild(li);
    });
  }
  
  // POST request: sends stored weather data to the forecast endpoint
  document.getElementById("postBtn").addEventListener("click", function(){
    fetch("http://localhost:5067/WeatherForecast/forecast", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(weatherDataArray)
    })
    .then(response => {
      if (!response.ok) {
        throw new Error("Hiba történt: " + response.status);
      }
      return response.json();
    })
    .then(data => {
      // NEW: display the forecast panel
      showForecast(data)
    })
    .catch(error => {
      resultDiv.innerHTML = `<p class="text-danger">Hiba: ${error.message}</p>`;
    });
  });
  
  // GET request: retrieves all stored weather data from the API
  document.getElementById("getBtn").addEventListener("click", function(){
    fetch("http://localhost:5067/WeatherForecast", {
      method: "GET"
    })
    .then(response => {
      if (!response.ok) {
        throw new Error("Hiba történt: " + response.status);
      }
      return response.json();
    })
    .then(data => {
      resultDiv.innerHTML = `<h3>Tárolt adatok:</h3>
        <pre>${JSON.stringify(data, null, 2)}</pre>`;
    })
    .catch(error => {
      resultDiv.innerHTML = `<p class="text-danger">Hiba: ${error.message}</p>`;
    });
  });

  // NEW: helper to show and style the forecast panel
  function showForecast(data) {
    // remove previous type‐classes
    ["napos","borult","esos","havas"].forEach(key => {
      forecastContainer.classList.remove(`bg-${key}`);
      previousContainer.classList.remove(`bg-${key}-list`);
    });

    const keys = ["napos","borult","esos","havas"];
    // IMPORTANT use lowercase type or else it doesn't work
    const key  = keys[data.type];

    // apply new backgrounds
    forecastContainer.classList.add(`bg-${key}`);
    previousContainer.classList.add(`bg-${key}-list`);

    // fill in the panel
    document.getElementById("forecastTemp").textContent     = data.temperature + "°C";
    // LOWERCASE "type"
    document.getElementById("forecastIcon").src            = `images/${weatherTypeIcons[data.type]}`;
    document.getElementById("forecastTypeName").textContent = getWeatherTypeName(data.type);
    document.getElementById("forecastWind").textContent     = data.windSpeed + " km/h";

    // show it
    forecastContainer.classList.remove("d-none");
  }

  // initial render of empty list
  updateDataList();
  