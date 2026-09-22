(function () {
  'use strict';

  const color = document.querySelector('#color');
  const bgColor = document.querySelector('#bgColor');
  const colorsTable = document.querySelector('#colorsTable tbody');
  const colorHistory = [];

  const updateColor = (type, value) => {
    document.body.style[type] = value;
    const now = new Date().toLocaleString();

    if (!colorHistory.length) {
      colorsTable.deleteRow(0);
    }

    const colorData = {
      time: now,
      fontColor: getComputedStyle(document.body).color,
      bgColor: getComputedStyle(document.body).backgroundColor
    };

    colorHistory.push(colorData);

    const row = colorsTable.insertRow();
    row.innerHTML = `<td>${colorData.time}</td>
                    <td>${colorData.fontColor}</td>
                    <td>${colorData.bgColor}</td>`

    row.addEventListener('click', () => {
      document.body.style.color = colorData.fontColor;
      document.body.style.backgroundColor = colorData.bgColor;
      console.log(`Text: ${colorData.fontColor}, Background: ${colorData.bgColor}`);
    });
  };

  color.addEventListener('change', () => updateColor('color', color.value));
  bgColor.addEventListener('change', () => updateColor('backgroundColor', bgColor.value));
}()); 