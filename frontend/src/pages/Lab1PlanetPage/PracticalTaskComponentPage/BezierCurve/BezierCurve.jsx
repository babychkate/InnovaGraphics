import React, { useEffect, useRef, useState } from 'react';

const BezierCurve = ({ rawPoints }) => {
    const canvasRef = useRef(null);
    const [scale, setScale] = useState(1);
    const [offset, setOffset] = useState({ x: 0, y: 0 });
    const [draggingPoint, setDraggingPoint] = useState(null);
    const [localPoints, setLocalPoints] = useState([]);
    const [isPanning, setIsPanning] = useState(false);
    const panStart = useRef({ x: 0, y: 0 });
    const offsetStart = useRef({ x: 0, y: 0 });
    const justDragged = useRef(false);

    useEffect(() => {
        setLocalPoints(rawPoints?.map(p => [...p]) || []);
    }, [rawPoints]);

    const factorial = (n) => {
        let result = 1;
        for (let i = 1; i <= n; i++) result *= i;
        return result;
    };

    const calculateBezierPoint = (t, points) => {
        const n = points.length - 1;
        let x = 0, y = 0;
        for (let i = 0; i <= n; i++) {
            const binomial = factorial(n) / (factorial(i) * factorial(n - i));
            const term = binomial * Math.pow(1 - t, n - i) * Math.pow(t, i);
            x += term * points[i][0];
            y += term * points[i][1];
        }
        return { x, y };
    };

    const getMouseCoords = (e) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = (e.clientX - rect.left) / scale - offset.x;
        const y = (e.clientY - rect.top) / scale - offset.y;
        return { x, y };
    };

    const handleMouseDown = (e) => {
        const { x, y } = getMouseCoords(e);
        for (let i = 0; i < localPoints.length; i++) {
            const [px, py] = localPoints[i];
            const dx = x - px;
            const dy = y - py;
            if (dx * dx + dy * dy <= 0.2) {
                setDraggingPoint(i);
                justDragged.current = false;
                return;
            }
        }

        setIsPanning(true);
        panStart.current = { x: e.clientX, y: e.clientY };
        offsetStart.current = { ...offset };
    };

    const handleMouseUp = () => {
        if (draggingPoint !== null) {
            justDragged.current = true;
        }
        setDraggingPoint(null);
        setIsPanning(false);
    };

    const handleMouseMove = (e) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = (e.clientX - rect.left) / scale - offset.x;
        const y = (e.clientY - rect.top) / scale - offset.y;

        let cursor = 'default';

        if (draggingPoint !== null) {
            setLocalPoints(prev =>
                prev.map((p, i) => (i === draggingPoint ? [x, y] : p))
            );
            cursor = 'grabbing';
        } else if (isPanning) {
            const dx = (e.clientX - panStart.current.x) / scale;
            const dy = (e.clientY - panStart.current.y) / scale;
            setOffset({
                x: offsetStart.current.x + dx,
                y: offsetStart.current.y + dy
            });
            cursor = 'move';
        } else {
            for (let i = 0; i < localPoints.length; i++) {
                const [px, py] = localPoints[i];
                const dx = x - px;
                const dy = y - py;
                if (dx * dx + dy * dy <= 0.2) {
                    cursor = 'grab';
                    break;
                }
            }
        }

        canvas.style.cursor = cursor;
    };

    const handleDoubleClick = (e) => {
        if (justDragged.current) {
            justDragged.current = false;
            return;
        }
        const { x, y } = getMouseCoords(e);
        setLocalPoints(prev => [...prev, [x, y]]);
    };

    const handleWheel = (e) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();

        const mouseX = e.clientX - rect.left;
        const mouseY = e.clientY - rect.top;

        const wheelDelta = -e.deltaY;
        const zoomIntensity = 0.0015;

        let newScale = scale * (1 + wheelDelta * zoomIntensity);
        newScale = Math.min(Math.max(1, newScale), 1000);

        const dx = (mouseX / scale) - offset.x;
        const dy = (mouseY / scale) - offset.y;

        const newOffsetX = (mouseX / newScale) - dx;
        const newOffsetY = (mouseY / newScale) - dy;

        setScale(newScale);
        setOffset({ x: newOffsetX, y: newOffsetY });
    };

    function drawGrid(ctx, width, height, scale, offsetX, offsetY) {
        ctx.clearRect(0, 0, width, height);

        const spacing = scale;

        // Визначення кроку для підписів сітки
        const minPixelSpacing = 40;
        const unitPixelSize = spacing;
        const steps = [1, 2, 5];
        let labelStep = 1;
        while (labelStep * unitPixelSize < minPixelSpacing) {
            const step = steps.shift();
            labelStep *= step;
            steps.push(step);
        }
        const gridStep = labelStep * spacing;

        ctx.beginPath();
        ctx.strokeStyle = '#ddd';
        ctx.lineWidth = 1;

        // Малюємо вертикальні лінії сітки
        let startX = offsetX * scale % gridStep;
        if (startX > 0) startX -= gridStep;
        for (let x = startX; x <= width; x += gridStep) {
            ctx.moveTo(x, 0);
            ctx.lineTo(x, height);
        }

        // Малюємо горизонтальні лінії сітки
        let startY = offsetY * scale % gridStep;
        if (startY > 0) startY -= gridStep;
        for (let y = startY; y <= height; y += gridStep) {
            ctx.moveTo(0, y);
            ctx.lineTo(width, y);
        }

        ctx.stroke();

        // Обчислюємо координати осей, прикріплені до меж канваса
        const axisX = Math.min(Math.max(offsetX * scale, 0), width);
        const axisY = Math.min(Math.max(offsetY * scale, 0), height);

        // Малюємо осі X і Y (товсті лінії)
        ctx.beginPath();
        ctx.strokeStyle = '#000';
        ctx.lineWidth = 2;

        // Вертикальна вісь Y (зафіксована в межах від 0 до width)
        ctx.moveTo(axisX, 0);
        ctx.lineTo(axisX, height);

        // Горизонтальна вісь X (зафіксована в межах від 0 до height)
        ctx.moveTo(0, axisY);
        ctx.lineTo(width, axisY);

        ctx.stroke();

        // Малюємо стрілки на осях
        const arrowSize = 10;
        ctx.beginPath();
        ctx.fillStyle = '#000';

        // Стрілка по X (праворуч)
        ctx.moveTo(width - arrowSize, axisY - arrowSize / 2);
        ctx.lineTo(width, axisY);
        ctx.lineTo(width - arrowSize, axisY + arrowSize / 2);
        ctx.closePath();
        ctx.fill();

        // Стрілка по Y (вгору)
        ctx.beginPath();
        ctx.moveTo(axisX - arrowSize / 2, arrowSize);
        ctx.lineTo(axisX, 0);
        ctx.lineTo(axisX + arrowSize / 2, arrowSize);
        ctx.closePath();
        ctx.fill();

        // Підписи чисел на осях через labelStep
        ctx.fillStyle = '#000';
        ctx.font = '12px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'top';

        // Підписи по осі X
        let startLabelX = Math.floor(-offsetX / labelStep) * labelStep;
        const endLabelX = Math.ceil((width / scale - offsetX) / labelStep) * labelStep;
        for (let x = startLabelX; x <= endLabelX; x += labelStep) {
            const px = (x + offsetX) * scale;
            ctx.fillText(x.toString(), px, axisY + 5);
        }

        // Підписи по осі Y
        ctx.textAlign = 'right';
        ctx.textBaseline = 'middle';

        let startLabelY = Math.floor(-offsetY / labelStep) * labelStep;
        const endLabelY = Math.ceil((height / scale - offsetY) / labelStep) * labelStep;
        for (let y = startLabelY; y <= endLabelY; y += labelStep) {
            const py = (y + offsetY) * scale;
            if (py !== axisY) ctx.fillText((-y).toString(), axisX - 5, py);
        }
    }

    const drawBezier = (ctx, points) => {
        if (points.length < 2) return;

        const steps = 200;

        // Крива Безьє — чорна
        ctx.beginPath();
        ctx.strokeStyle = '#000000';
        ctx.lineWidth = 3;
        ctx.lineJoin = 'round';

        for (let i = 0; i <= steps; i++) {
            const t = i / steps;
            const { x, y } = calculateBezierPoint(t, points);
            const cx = (x + offset.x) * scale;
            const cy = (y + offset.y) * scale;
            if (i === 0) ctx.moveTo(cx, cy);
            else ctx.lineTo(cx, cy);
        }
        ctx.stroke();

        // Допоміжні лінії
        ctx.strokeStyle = 'rgba(0,0,255,0.3)';
        ctx.lineWidth = 1.5;
        ctx.setLineDash([6, 8]);
        ctx.beginPath();
        for (let i = 0; i < points.length; i++) {
            const [x, y] = points[i];
            const cx = (x + offset.x) * scale;
            const cy = (y + offset.y) * scale;
            if (i === 0) ctx.moveTo(cx, cy);
            else ctx.lineTo(cx, cy);
        }
        ctx.stroke();
        ctx.setLineDash([]);

        // Точки: червона для першої та останньої, інші — сині
        for (let i = 0; i < points.length; i++) {
            const [x, y] = points[i];
            const cx = (x + offset.x) * scale;
            const cy = (y + offset.y) * scale;
            ctx.beginPath();
            ctx.arc(cx, cy, 5, 0, 2 * Math.PI);
            ctx.fillStyle = (i === 0 || i === points.length - 1) ? '#FF0000' : '#0000FF';
            ctx.fill();
        }
    };

    useEffect(() => {
        const canvas = canvasRef.current;
        const ctx = canvas.getContext('2d');
        const width = canvas.width;
        const height = canvas.height;

        drawGrid(ctx, width, height, scale, offset.x, offset.y);
        if (localPoints.length > 0) drawBezier(ctx, localPoints);
    }, [localPoints, scale, offset]);

    return (
        <canvas
            ref={canvasRef}
            width={750}
            height={678}
            onMouseDown={handleMouseDown}
            onMouseMove={handleMouseMove}
            onMouseUp={handleMouseUp}
            onDoubleClick={handleDoubleClick}
            onWheel={handleWheel}
        />
    );
};

export default BezierCurve;