window.canvasInterop = {
    initializeCanvas: (canvasId) => {
        const canvas = document.getElementById(canvasId);
        const container = canvas.parentElement;
        const ctx = canvas.getContext('2d');

        function resizeCanvas() {
            const containerWidth = container.clientWidth;
            const containerHeight = container.clientHeight;

            canvas.style.width = `${containerWidth}px`;
            canvas.style.height = `${containerHeight}px`;

            const scale = window.devicePixelRatio || 1;
            canvas.width = containerWidth * scale;
            canvas.height = containerHeight * scale;

            ctx.scale(scale, scale);
        }

        window.addEventListener('resize', resizeCanvas);
        resizeCanvas();

        return true;
    },
    drawRect: (canvasId, x, y, width, height, color) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');

        ctx.fillStyle = color;
        ctx.fillRect(x, y, width, height);
    },
    drawCircle: (canvasId, x, y, radius, color) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');

        ctx.fillStyle = color;
        ctx.beginPath();
        ctx.arc(x, y, radius, 0, 2 * Math.PI);
        ctx.fill();
    },
    moveTo: (canvasId, x, y) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');
        
        if (ctx) {
            ctx.beginPath();
            ctx.moveTo(x, y);
        }
    },

    lineTo: (canvasId, x, y) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');
        
        if (ctx) {
            ctx.lineTo(x, y);
        }
    },

    stroke: (canvasId, color, lineWidth) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');
        
        if (ctx) {
            ctx.strokeStyle = color || "black";
            ctx.lineWidth = lineWidth || 1;
            ctx.stroke();
        }
    },

    clearCanvas: (canvasId) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');

        if (ctx) {
            ctx.clearRect(0, 0, canvas.width, canvas.height);  // Clear the canvas
        }
    },
    
    animateCanvas: (canvasId, dotnetHelper) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext('2d');

        function animate() {
            dotnetHelper.invokeMethodAsync('DrawAllLinesAsync', 'canvas');
            requestAnimationFrame(animate);
        }

        animate();
    }
};