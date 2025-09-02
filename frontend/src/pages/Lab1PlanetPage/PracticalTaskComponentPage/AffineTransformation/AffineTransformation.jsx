import React, { useRef, useState, useEffect } from 'react';
import { useSelector } from 'react-redux';

const AffineTransformation = ({ setInputPoints }) => {
    const { expectedPoints, realPoints } = useSelector(state => state.code);

    const canvasRefOriginal = useRef(null);
    const canvasRefTransformed = useRef(null);
    const canvasRefExpected = useRef(null);

    const [scaleOriginal, setScaleOriginal] = useState(50);
    const [scaleTransformed, setScaleTransformed] = useState(50);
    const [scaleExpected, setScaleExpected] = useState(50);

    const [points, setPoints] = useState([
        [1, 1],
        [-1, 1],
        [0, -1],
    ]);
    setInputPoints(points);

    const [draggingIndex, setDraggingIndex] = useState(null);

    const getMousePos = (e, canvas) => {
        const rect = canvas.getBoundingClientRect();
        const scaleX = canvas.width / rect.width;
        const scaleY = canvas.height / rect.height;
        return {
            x: (e.clientX - rect.left) * scaleX,
            y: (e.clientY - rect.top) * scaleY,
        };
    };

    const drawArrow = (ctx, fromX, fromY, toX, toY, arrowLength = 10) => {
        const angle = Math.atan2(toY - fromY, toX - fromX);
        ctx.beginPath();
        ctx.moveTo(toX, toY);
        ctx.lineTo(
            toX - arrowLength * Math.cos(angle - Math.PI / 6),
            toY - arrowLength * Math.sin(angle - Math.PI / 6)
        );
        ctx.lineTo(
            toX - arrowLength * Math.cos(angle + Math.PI / 6),
            toY - arrowLength * Math.sin(angle + Math.PI / 6)
        );
        ctx.closePath();
        ctx.fill();
    };

    const drawGrid = (ctx, width, height, scale) => {
        ctx.clearRect(0, 0, width, height);
        const centerX = width / 2;
        const centerY = height / 2;

        ctx.save();
        ctx.translate(centerX, centerY);
        ctx.scale(1, -1);

        ctx.strokeStyle = '#e0e0e0';
        ctx.lineWidth = 1;
        ctx.font = '16px Arial';
        ctx.fillStyle = '#888';

        let step;
        if (scale >= 50) step = scale * 1;
        else if (scale >= 25) step = scale * 2;  
        else if (scale >= 10) step = scale * 5;  
        else step = scale * 10;                  

        for (let x = step; x <= centerX; x += step) {
            ctx.beginPath();
            ctx.moveTo(x, -centerY);
            ctx.lineTo(x, centerY);
            ctx.stroke();
            ctx.save();
            ctx.scale(1, -1);
            ctx.fillText(`${(x / scale).toFixed()}`, x + 2, -6);
            ctx.restore();
        }
        for (let x = -step; x >= -centerX; x -= step) {
            ctx.beginPath();
            ctx.moveTo(x, -centerY);
            ctx.lineTo(x, centerY);
            ctx.stroke();
            ctx.save();
            ctx.scale(1, -1);
            ctx.fillText(`${(x / scale).toFixed()}`, x + 2, -6);
            ctx.restore();
        }
        for (let y = step; y <= centerY; y += step) {
            ctx.beginPath();
            ctx.moveTo(-centerX, y);
            ctx.lineTo(centerX, y);
            ctx.stroke();
            ctx.save();
            ctx.scale(1, -1);
            ctx.fillText(`${(-y / scale).toFixed()}`, 4, -y - 4);
            ctx.restore();
        }
        for (let y = -step; y >= -centerY; y -= step) {
            ctx.beginPath();
            ctx.moveTo(-centerX, y);
            ctx.lineTo(centerX, y);
            ctx.stroke();
            ctx.save();
            ctx.scale(1, -1);
            ctx.fillText(`${(-y / scale).toFixed()}`, 4, -y - 4);
            ctx.restore();
        }

        ctx.strokeStyle = '#000';
        ctx.beginPath();
        ctx.moveTo(-centerX, 0);
        ctx.lineTo(centerX, 0);
        ctx.stroke();
        drawArrow(ctx, centerX - 20, 0, centerX, 0);

        ctx.beginPath();
        ctx.moveTo(0, -centerY);
        ctx.lineTo(0, centerY);
        ctx.stroke();
        drawArrow(ctx, 0, centerY - 20, 0, centerY);

        ctx.restore();
    };

    const drawPoints = (ctx, width, height, scale, data, color = 'black') => {
        const centerX = width / 2;
        const centerY = height / 2;

        ctx.save();
        ctx.translate(centerX, centerY);
        ctx.scale(1, -1);

        ctx.fillStyle = color;
        ctx.font = '10px Arial';
        data.forEach(([x, y]) => {
            const px = x * scale;
            const py = y * scale;
            ctx.beginPath();
            ctx.arc(px, py, 4, 0, 2 * Math.PI);
            ctx.fill();
            ctx.save();
            ctx.scale(1, -1);
            ctx.fillText(`(${x.toFixed()}, ${y.toFixed()})`, px + 8, -py - 8);
            ctx.restore();
        });

        ctx.restore();
    };

    const drawPolygon = (ctx, width, height, scale, data, color = 'black') => {
        if (!data || data.length < 2) return;

        const centerX = width / 2;
        const centerY = height / 2;

        ctx.save();
        ctx.translate(centerX, centerY);
        ctx.scale(1, -1);

        ctx.strokeStyle = color;
        ctx.lineWidth = 2;
        ctx.beginPath();

        data.forEach(([x, y], i) => {
            const px = x * scale;
            const py = y * scale;
            if (i === 0) {
                ctx.moveTo(px, py);
            } else {
                ctx.lineTo(px, py);
            }
        });

        ctx.closePath();
        ctx.stroke();

        ctx.restore();
    };

    const findPointIndexAtPos = (pos, points, scale, canvas) => {
        const centerX = canvas.width / 2;
        const centerY = canvas.height / 2;

        for (let i = 0; i < points.length; i++) {
            const [x, y] = points[i];
            const px = centerX + x * scale;
            const py = centerY - y * scale;
            const dist = Math.hypot(px - pos.x, py - pos.y);
            if (dist < 10) return i;
        }
        return null;
    };

    const handleMouseDown = (e) => {
        const canvas = canvasRefOriginal.current;
        const pos = getMousePos(e, canvas);
        const idx = findPointIndexAtPos(pos, points, scaleOriginal, canvas);
        if (idx !== null) {
            setDraggingIndex(idx);
        }
    };

    const handleMouseMove = (e) => {
        if (draggingIndex === null) return;

        const canvas = canvasRefOriginal.current;
        const pos = getMousePos(e, canvas);

        const centerX = canvas.width / 2;
        const centerY = canvas.height / 2;

        const logicX = (pos.x - centerX) / scaleOriginal;
        const logicY = (centerY - pos.y) / scaleOriginal;

        const newPoints = [...points];
        newPoints[draggingIndex] = [logicX, logicY];
        console.log(newPoints);
        setPoints(newPoints);
        setInputPoints(newPoints);
    };

    const handleMouseUp = () => {
        setDraggingIndex(null);
    };

    const handleWheel = (e, type) => {
        e.preventDefault();
        const delta = e.deltaY < 0 ? 5 : -5;

        switch (type) {
            case 'original':
                setScaleOriginal(prev => Math.max(10, prev + delta));
                break;
            case 'transformed':
                setScaleTransformed(prev => Math.max(10, prev + delta));
                break;
            case 'expected':
                setScaleExpected(prev => Math.max(10, prev + delta));
                break;
            default:
                break;
        }
    };

    useEffect(() => {
        const canvas = canvasRefOriginal.current;
        const ctx = canvas.getContext('2d');
        drawGrid(ctx, canvas.width, canvas.height, scaleOriginal);
        drawPolygon(ctx, canvas.width, canvas.height, scaleOriginal, points, 'black');
        drawPoints(ctx, canvas.width, canvas.height, scaleOriginal, points, 'black');
    }, [points, scaleOriginal]);

    useEffect(() => {
        const canvas = canvasRefTransformed.current;
        const ctx = canvas.getContext('2d');
        drawGrid(ctx, canvas.width, canvas.height, scaleTransformed);
        if (realPoints?.length) {
            drawPolygon(ctx, canvas.width, canvas.height, scaleTransformed, realPoints, 'blue');
            drawPoints(ctx, canvas.width, canvas.height, scaleTransformed, realPoints, 'blue');
        }
    }, [realPoints, scaleTransformed]);

    useEffect(() => {
        const canvas = canvasRefExpected.current;
        const ctx = canvas.getContext('2d');
        drawGrid(ctx, canvas.width, canvas.height, scaleExpected);
        if (expectedPoints?.length) {
            drawPolygon(ctx, canvas.width, canvas.height, scaleExpected, expectedPoints, 'green');
            drawPoints(ctx, canvas.width, canvas.height, scaleExpected, expectedPoints, 'green');
        }
    }, [expectedPoints, scaleExpected]);

    return (
        <div className="w-full flex flex-col gap-4 p-4">
            <div className="flex flex-row gap-4 w-full">
                <div
                    className="flex flex-col w-1/2"
                    onMouseDown={handleMouseDown}
                    onMouseMove={handleMouseMove}
                    onMouseUp={handleMouseUp}
                    onMouseLeave={handleMouseUp}
                >
                    <h3>Початковий малюнок (перетягуйте точки)</h3>
                    <canvas
                        ref={canvasRefOriginal}
                        width={300}
                        height={180}
                        onWheel={(e) => handleWheel(e, 'original')}
                    />
                </div>
                <div className="flex flex-col w-1/2">
                    <h3>Малюнок після афінного перетворення</h3>
                    <canvas
                        ref={canvasRefTransformed}
                        width={300}
                        height={180}
                        onWheel={(e) => handleWheel(e, 'transformed')}
                    />
                </div>
            </div>

            <div className="flex flex-col items-start w-2/3">
                <h3>Очікуваний малюнок</h3>
                <canvas
                    ref={canvasRefExpected}
                    width={370}
                    height={300}
                    style={{ backgroundColor: 'white' }}
                    onWheel={(e) => handleWheel(e, 'expected')}
                />
            </div>
        </div>
    );
};

export default AffineTransformation;