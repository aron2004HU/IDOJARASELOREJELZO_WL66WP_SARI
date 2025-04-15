// Returns the numeric value corresponding to the WeatherType
// 0: Napos, 1: Borult, 2: Esos, 3: Havas
function getEnumValue(selectedWeather) {
    switch (selectedWeather) {
      case "Napos": return 0;
      case "Borult": return 1;
      case "Esos": return 2;
      case "Havas": return 3;
      default: return 0;
    }
  }
  
  // Global array to store weather data (for POST payload)
  let weatherDataArray = [];
  const dataList = document.getElementById("dataList");
  const resultDiv = document.getElementById("result");
  
  // Event handler for "Adat hozzáadása" button: Convert selection to a numeric value and add to the array
  document.getElementById("addBtn").addEventListener("click", function(){
    const select = document.getElementById("weatherSelect");
    const selectedWeather = select.value; // "Napos", "Borult", "Esos", "Havas"
    weatherDataArray.push({ "Type": getEnumValue(selectedWeather) });
    updateDataList();
  });
  
  
  
  // Updates the displayed list of stored weather data
  function updateDataList(){
    dataList.innerHTML = "";
    weatherDataArray.forEach((data, index) => {
      const li = document.createElement("li");
      li.className = "list-group-item";
      li.textContent = "Adat " + (index + 1) + ": " + data.Type;
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
      resultDiv.innerHTML = `<h3>Előrejelzés eredménye:</h3>
        <pre>${JSON.stringify(data, null, 2)}</pre>`;
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
  