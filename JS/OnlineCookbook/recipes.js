(function () {
    'use strict';

    const recipeChoiceDiv = document.querySelector('#recipeChoice');

    const recipeDisplay = document.querySelector('#display');
    const heading = document.createElement('h1');
    heading.textContent = 'No Recipe Chosen';
    recipeDisplay.appendChild(heading);

    const img = document.createElement('img');
    img.src = 'media/onlineCookbook.png';
    img.alt = 'The Online Cookbook';
    recipeDisplay.appendChild(img);

    const recipeDetails = document.createElement('div');
    recipeDetails.id = 'recipeDetails';
    recipeDisplay.appendChild(recipeDetails);

    const ingredientDiv = document.createElement('div');
    ingredientDiv.id = 'ingredientDiv';
    const ingredients = document.createElement('h3');
    ingredients.textContent = 'Ingredients:';
    ingredientDiv.appendChild(ingredients);
    recipeDetails.appendChild(ingredientDiv);

    const instructionDiv = document.createElement('div');
    instructionDiv.id = 'instructionDiv';
    const instructions = document.createElement('h3');
    instructions.textContent = 'Instructions:';
    instructionDiv.appendChild(instructions);
    recipeDetails.appendChild(instructionDiv);

    const fetchRecipes = async () => {
        try {
            const response = await fetch('recipes.json');
            if (!response.ok) {
                throw new Error(`${response.status} - ${response.statusText}`);
            }
            const contents = await response.json();
            //console.log(contents);

            contents.forEach(r => {
                const radioInput = document.createElement('input');
                radioInput.type = 'radio';
                radioInput.id = `${r.id}`;
                radioInput.name = 'recipe';
                radioInput.value = `${r.id}`;

                const label = document.createElement('label');
                label.htmlFor = `${r.id}`;
                label.textContent = `${r.name}`;
                
                recipeChoiceDiv.appendChild(radioInput);
                recipeChoiceDiv.appendChild(label);

                document.querySelector(`#${r.id}`).addEventListener('change', async () => {
                    const file = `${r.id}.json`;
                    try {
                        const response = await fetch(file);
                        if (!response.ok) {
                            throw new Error(`${response.status} - ${response.statusText}`);
                        }
                        const contents = await response.json();
                        //console.log(`${r.id}.json:`, contents);

                        heading.textContent = contents[0].title;
                        img.src = contents[0].picture;
                        img.alt = contents[0].title;
                        ingredientDiv.innerHTML = '<h3>Ingredients:</h3>';
                        instructionDiv.innerHTML = '<h3>Instructions:</h3>';
                        const ingredientList = document.createElement('ul');
                        contents[0].ingredients.forEach(ingredient => {
                            const li = document.createElement('li');
                            li.textContent = ingredient;
                            ingredientList.appendChild(li);
                        });
                        ingredientDiv.appendChild(ingredientList);
                        contents[0].instructions.forEach((instruction) => {
                            const p = document.createElement('p');
                            p.textContent = `${instruction}`;
                            instructionDiv.appendChild(p);
                        });
                    } catch (e) {
                        console.error('oops', e);
                        heading.textContent = 'Error loading recipe. Please try again.';
                        img.src = 'media/onlineCookbook.png';
                        img.alt = 'The Online Cookbook';
                        ingredientDiv.innerHTML = '<h3>Ingredients:</h3><p>No ingredients available.</p>';
                        instructionDiv.innerHTML = '<h3>Instructions:</h3><p>No instructions available.</p>';
                    }
                });
            });

        } catch (e) {
            console.error('oops', e);
        }
    };
    fetchRecipes();
}());