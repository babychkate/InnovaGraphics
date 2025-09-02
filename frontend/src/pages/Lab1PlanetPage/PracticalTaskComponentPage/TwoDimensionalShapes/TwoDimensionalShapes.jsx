import React, { useEffect, useRef, useState } from 'react';

const TwoDimensionalShapes = ({ rawPoints }) => {
    const canvasRef = useRef(null);
    const [scale, setScale] = useState(25);
    console.log(rawPoints);

    useEffect(() => {
        const canvas = canvasRef.current;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        if (!rawPoints || rawPoints.length < 7) return;

        const centerX = canvas.width / 2;
        const centerY = canvas.height / 2;

        const drawPoint = ([x, y]) => ({
            x: centerX + x * scale,
            y: centerY - y * scale,
        });

        drawGrid(ctx, canvas.width, canvas.height, scale, centerX, centerY);

        // Малюємо трикутник
        const trianglePoints = rawPoints.slice(0, 3).map(drawPoint);
        ctx.beginPath();
        ctx.moveTo(trianglePoints[0].x, trianglePoints[0].y);
        trianglePoints.forEach(p => ctx.lineTo(p.x, p.y));
        ctx.closePath();
        ctx.strokeStyle = 'blue';
        ctx.lineWidth = 2;
        ctx.stroke();
        ctx.fillStyle = 'rgba(0, 0, 255, 0.2)';
        ctx.fill();

        // Малюємо прямокутник
        const rectanglePoints = rawPoints.slice(3, 7).map(drawPoint);
        ctx.beginPath();
        ctx.moveTo(rectanglePoints[0].x, rectanglePoints[0].y);
        rectanglePoints.forEach(p => ctx.lineTo(p.x, p.y));
        ctx.closePath();
        ctx.strokeStyle = 'green';
        ctx.lineWidth = 2;
        ctx.stroke();
        ctx.fillStyle = 'rgba(0, 255, 0, 0.2)';
        ctx.fill();
    }, [rawPoints, scale]);

    useEffect(() => {
        const handleWheel = (e) => {
            e.preventDefault();
            const delta = Math.sign(e.deltaY);
            setScale(prev => {
                const newScale = delta < 0 ? prev * 1.1 : prev / 1.1;
                return Math.max(5, Math.min(200, newScale));
            });
        };

        const canvas = canvasRef.current;
        canvas.addEventListener('wheel', handleWheel, { passive: false });

        return () => {
            canvas.removeEventListener('wheel', handleWheel);
        };
    }, []);

    const drawGrid = (ctx, width, height, scale, centerX, centerY) => {
        const spacing = scale;

        ctx.beginPath();
        ctx.strokeStyle = '#ddd';
        ctx.lineWidth = 1;

        // Вертикальні лінії
        for (let x = centerX % spacing; x <= width; x += spacing) {
            ctx.moveTo(x, 0);
            ctx.lineTo(x, height);
        }
        for (let x = centerX - spacing; x >= 0; x -= spacing) {
            ctx.moveTo(x, 0);
            ctx.lineTo(x, height);
        }

        // Горизонтальні лінії
        for (let y = centerY % spacing; y <= height; y += spacing) {
            ctx.moveTo(0, y);
            ctx.lineTo(width, y);
        }
        for (let y = centerY - spacing; y >= 0; y -= spacing) {
            ctx.moveTo(0, y);
            ctx.lineTo(width, y);
        }

        ctx.stroke();

        // Основні осі
        ctx.beginPath();
        ctx.strokeStyle = '#000';
        ctx.lineWidth = 1.5;
        ctx.moveTo(0, centerY); // X
        ctx.lineTo(width, centerY);
        ctx.moveTo(centerX, 0); // Y
        ctx.lineTo(centerX, height);
        ctx.stroke();

        // Стрілки
        const arrowSize = 10;
        ctx.beginPath();
        ctx.fillStyle = '#000';
        ctx.moveTo(width, centerY);
        ctx.lineTo(width - arrowSize, centerY - arrowSize / 2);
        ctx.lineTo(width - arrowSize, centerY + arrowSize / 2);
        ctx.closePath();
        ctx.fill();

        ctx.beginPath();
        ctx.moveTo(centerX, 0);
        ctx.lineTo(centerX - arrowSize / 2, arrowSize);
        ctx.lineTo(centerX + arrowSize / 2, arrowSize);
        ctx.closePath();
        ctx.fill();

        // Написи X та Y
        ctx.fillStyle = '#000';
        ctx.font = '16px Arial';
        ctx.fillText('X', width - arrowSize - 15, centerY - 8);
        ctx.fillText('Y', centerX + 8, arrowSize + 12);

        const minPixelSpacing = 40;
        let labelStep = 1;
        const unitPixelSize = scale;
        const steps = [1, 2, 5];

        while (labelStep * unitPixelSize < minPixelSpacing) {
            const step = steps.shift();
            labelStep *= step;
            steps.push(step);
        }

        ctx.font = '12px Arial';
        ctx.fillStyle = '#000';

        // X-вісь (вліво і вправо)
        for (let i = 0; centerX + i * scale <= width; i++) {
            const x = centerX + i * scale;
            if (i * 1 % labelStep === 0 && i !== 0) {
                ctx.fillText((i).toString(), x - 3, centerY + 15);
            }
            ctx.beginPath();
            ctx.moveTo(x, centerY - 4);
            ctx.lineTo(x, centerY + 4);
            ctx.stroke();
        }

        for (let i = 1; centerX - i * scale >= 0; i++) {
            const x = centerX - i * scale;
            if (i * -1 % labelStep === 0) {
                ctx.fillText((-i).toString(), x - 5, centerY + 15);
            }
            ctx.beginPath();
            ctx.moveTo(x, centerY - 4);
            ctx.lineTo(x, centerY + 4);
            ctx.stroke();
        }

        // Y-вісь (вгору і вниз)
        for (let i = 1; centerY - i * scale >= 0; i++) {
            const y = centerY - i * scale;
            if (i % labelStep === 0) {
                ctx.fillText(i.toString(), centerX + 5, y + 4);
            }
            ctx.beginPath();
            ctx.moveTo(centerX - 4, y);
            ctx.lineTo(centerX + 4, y);
            ctx.stroke();
        }

        for (let i = 1; centerY + i * scale <= height; i++) {
            const y = centerY + i * scale;
            if (-i % labelStep === 0) {
                ctx.fillText((-i).toString(), centerX + 5, y + 4);
            }
            ctx.beginPath();
            ctx.moveTo(centerX - 4, y);
            ctx.lineTo(centerX + 4, y);
            ctx.stroke();
        }

        // 0 у центрі
        ctx.fillText('0', centerX + 5, centerY + 15);
    };

    return (
        <div className="w-full">
            <canvas
                ref={canvasRef}
                width={750}
                height={678}
                style={{ cursor: 'grab', backgroundColor: 'white' }}
            />
        </div>
    );
};

export default TwoDimensionalShapes;