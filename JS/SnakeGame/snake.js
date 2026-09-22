(async function () {
    'use strict';

    const CELL_SIZE = 64;
    const SCORE_BAR_HEIGHT = 50;
    const INITIAL_SPEED = 500;
    const MIN_SPEED = 120;
    const SPEED_MULTIPLIER = 0.95;

    const theCanvas = document.querySelector('#theCanvas');
    const context = theCanvas.getContext('2d');
    const scoreDisplay = document.querySelector('#scoreValue');

    const gameOverSound = new Audio('media/gameOver.wav');
    const appleCrunch = new Audio('media/appleCrunch.mp3');

    // Load all images first
    const [snakeHeadImg, appleImg, tileA, tileB] = await Promise.all([
        loadImage('media/snakeHead.png'),
        loadImage('media/apple.png'),
        loadImage('media/tileA.jpg'),
        loadImage('media/tileB.jpg')
    ]);

    let checkeredPattern = null;

    function createCheckeredPattern() {
        const pat = document.createElement('canvas');
        pat.width = CELL_SIZE * 2;
        pat.height = CELL_SIZE * 2;
        const ctx = pat.getContext('2d');
        ctx.drawImage(tileA, 0, 0, CELL_SIZE, CELL_SIZE);
        ctx.drawImage(tileB, CELL_SIZE, 0, CELL_SIZE, CELL_SIZE);
        ctx.drawImage(tileB, 0, CELL_SIZE, CELL_SIZE, CELL_SIZE);
        ctx.drawImage(tileA, CELL_SIZE, CELL_SIZE, CELL_SIZE, CELL_SIZE);
        checkeredPattern = context.createPattern(pat, 'repeat');
    }

    function resizeCanvas() {
        const availableHeight = window.innerHeight - SCORE_BAR_HEIGHT;
        theCanvas.width = window.innerWidth - (window.innerWidth % CELL_SIZE);
        theCanvas.height = availableHeight - (availableHeight % CELL_SIZE);
        createCheckeredPattern();
    }

    resizeCanvas();
    window.addEventListener('resize', resizeCanvas);

    class Snake {
        constructor() {
            this.segments = [{ x: 0, y: 0 }];
            this.direction = 'ArrowRight';
            this.pendingDirection = 'ArrowRight';
        }

        get head() { return this.segments[0]; }

        changeDirection(dir) {
            const opposites = {
                ArrowRight: 'ArrowLeft',
                ArrowLeft: 'ArrowRight',
                ArrowUp: 'ArrowDown',
                ArrowDown: 'ArrowUp'
            };
            if (this.segments.length > 1 && opposites[dir] === this.direction) return;
            this.pendingDirection = dir;
        }

        updateDirection() {
            this.direction = this.pendingDirection;
        }

        move() {
            this.updateDirection();

            const dx = this.direction === 'ArrowRight' ? CELL_SIZE :
                this.direction === 'ArrowLeft' ? -CELL_SIZE : 0;
            const dy = this.direction === 'ArrowDown' ? CELL_SIZE :
                this.direction === 'ArrowUp' ? -CELL_SIZE : 0;

            const newHead = { x: this.head.x + dx, y: this.head.y + dy };

            if (newHead.x < 0 || newHead.x >= theCanvas.width ||
                newHead.y < 0 || newHead.y >= theCanvas.height) {
                return false;
            }

            if (this.segments.some(s => s.x === newHead.x && s.y === newHead.y)) {
                return false;
            }

            this.segments.unshift(newHead);
            return true;
        }

        grow() {
            const tail = this.segments[this.segments.length - 1];
            this.segments.push({ x: tail.x, y: tail.y });
        }

        removeTail() {
            this.segments.pop();
        }

        draw() {
            context.drawImage(snakeHeadImg, this.head.x, this.head.y, CELL_SIZE, CELL_SIZE);

            context.fillStyle = '#228B22';
            context.strokeStyle = 'darkgreen';
            context.lineWidth = 3;
            for (let i = 1; i < this.segments.length; i++) {
                const s = this.segments[i];
                context.fillRect(s.x + 4, s.y + 4, CELL_SIZE - 8, CELL_SIZE - 8);
                context.strokeRect(s.x + 4, s.y + 4, CELL_SIZE - 8, CELL_SIZE - 8);
            }
        }
    }

    class Apple {
        constructor() {
            this.x = 0;
            this.y = 0;
            this.exists = false;
        }

        place(occupied) {
            if (this.exists) return;

            const cols = theCanvas.width / CELL_SIZE;
            const rows = theCanvas.height / CELL_SIZE;
            let x, y;
            do {
                x = Math.floor(Math.random() * cols) * CELL_SIZE;
                y = Math.floor(Math.random() * rows) * CELL_SIZE;
            } while (occupied.some(s => s.x === x && s.y === y));

            this.x = x;
            this.y = y;
            this.exists = true;
        }

        draw() {
            if (this.exists) {
                context.drawImage(appleImg, this.x, this.y, CELL_SIZE, CELL_SIZE);
            }
        }

        isEaten(x, y) {
            return this.exists && this.x === x && this.y === y;
        }

        reset() {
            this.exists = false;
        }
    }

    const snake = new Snake();
    const apple = new Apple();

    let score = 0;
    let speed = INITIAL_SPEED;
    let gameLoop = null;

    function drawBackground() {
        context.fillStyle = checkeredPattern || '#8B7355';
        context.fillRect(0, 0, theCanvas.width, theCanvas.height);
    }

    function gameOver() {
        gameOverSound.play();

        apple.draw();
        snake.draw();

        context.fillStyle = 'rgba(0,0,0,0.75)';
        context.fillRect(0, 0, theCanvas.width, theCanvas.height);

        context.font = 'bold 80px Arial';
        context.fillStyle = 'white';
        context.strokeStyle = 'black';
        context.lineWidth = 8;
        context.textAlign = 'center';
        context.textBaseline = 'middle';
        context.strokeText('GAME OVER', theCanvas.width / 2, theCanvas.height / 2 - 40);
        context.fillText('GAME OVER', theCanvas.width / 2, theCanvas.height / 2 - 40);

        context.font = '36px Arial';
        context.strokeText(`Score: ${score}`, theCanvas.width / 2, theCanvas.height / 2 + 20);
        context.fillText(`Score: ${score}`, theCanvas.width / 2, theCanvas.height / 2 + 20);

        clearInterval(gameLoop);
        gameLoop = null;
    }

    function tick() {
        context.clearRect(0, 0, theCanvas.width, theCanvas.height);
        drawBackground();

        if (!snake.move()) {
            gameOver();
            return;
        }

        if (!apple.exists) apple.place(snake.segments);

        const eating = apple.isEaten(snake.head.x, snake.head.y);

        if (eating) {
            appleCrunch.play();
            score++;
            scoreDisplay.textContent = score;
            apple.reset();

            speed = Math.max(MIN_SPEED, speed * SPEED_MULTIPLIER);
            clearInterval(gameLoop);
            gameLoop = setInterval(tick, speed);
        } else {
            snake.removeTail();
        }

        apple.draw();
        snake.draw();
    }

    function startGame() {
        score = 0;
        speed = INITIAL_SPEED;
        scoreDisplay.textContent = '0';
        snake.segments = [{ x: 0, y: 0 }];
        snake.direction = snake.pendingDirection = 'ArrowRight';
        apple.reset();

        clearInterval(gameLoop);
        gameLoop = setInterval(tick, speed);
    }

    document.addEventListener('keydown', e => {
        if (['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'].includes(e.key)) {
            e.preventDefault();
            snake.changeDirection(e.key);
        }
    });

    startGame();

    function loadImage(src) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.src = src;
            img.onload = () => resolve(img);
            img.onerror = () => reject(new Error(`Failed to load ${src}`));
        });
    }
})();