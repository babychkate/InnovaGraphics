import React, { useRef, useEffect, useState } from 'react';

const Fractals = ({ rawPoints }) => {
    const canvasRef = useRef(null);
    const animationRefs = useRef({ left: null, right: null });

    const [scale, setScale] = useState(0.5);
    const [offset, setOffset] = useState({ x: 0, y: 0 });
    const [isDragging, setIsDragging] = useState(false);
    const [lastPos, setLastPos] = useState({ x: 0, y: 0 });
    const [hasAnimated, setHasAnimated] = useState(false);

    const drawLine = (ctx, start, end, scale, offset) => {
        ctx.beginPath();
        ctx.moveTo(start[0] * scale + offset.x, start[1] * scale + offset.y);
        ctx.lineTo(end[0] * scale + offset.x, end[1] * scale + offset.y);
        ctx.strokeStyle = '#8B008B';
        ctx.lineWidth = 1;
        ctx.stroke();
    };

    const drawFractal = (ctx, points, scale, offset) => {
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        for (let i = 0; i + 1 < points.length; i += 2) {
            drawLine(ctx, points[i], points[i + 1], scale, offset);
        }
    };

    const animateBranch = (ctx, points, scale, offset, animationKey) => {
        return new Promise((resolve) => {
            let index = 0;
            const linesPerFrame = 5;

            const step = () => {
                let drawn = 0;
                while (index + 1 < points.length && drawn < linesPerFrame) {
                    drawLine(ctx, points[index], points[index + 1], scale, offset);
                    index += 2;
                    drawn++;
                }

                if (index + 1 >= points.length) {
                    resolve();
                    return;
                }

                animationRefs.current[animationKey] = requestAnimationFrame(step);
            };

            step();
        });
    };

    const animateFromRoot = async (ctx, points, scale, offset) => {
        if (points.length < 2) return;

        drawLine(ctx, points[0], points[1], scale, offset);

        const mid = Math.floor(points.length / 2 / 2) * 2;
        const leftPoints = points.slice(0, mid);
        const rightPoints = points.slice(mid);

        const leftSubPoints = leftPoints.length > 2 ? leftPoints.slice(2) : [];
        const rightSubPoints = rightPoints.length > 2 ? rightPoints.slice(2) : [];

        await Promise.all([
            animateBranch(ctx, leftSubPoints, scale, offset, 'left'),
            animateBranch(ctx, rightSubPoints, scale, offset, 'right'),
        ]);
    };

    useEffect(() => {
        const canvas = canvasRef.current;
        if (!canvas || !rawPoints || rawPoints.length === 0) return;

        const ctx = canvas.getContext('2d');

        let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;
        rawPoints.forEach(([x, y]) => {
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        });

        const centerX = (minX + maxX) / 2;
        const centerY = (minY + maxY) / 2;

        const canvasCenterX = canvas.width / 2;
        const canvasCenterY = canvas.height / 2;

        const centeredOffset = {
            x: canvasCenterX - centerX * scale,
            y: canvasCenterY - centerY * scale,
        };

        setOffset(centeredOffset);

        if (!hasAnimated) {
            cancelAnimationFrame(animationRefs.current.left);
            cancelAnimationFrame(animationRefs.current.right);
            animateFromRoot(ctx, rawPoints, scale, centeredOffset).then(() => {
                setHasAnimated(true);
            });
        } else {
            drawFractal(ctx, rawPoints, scale, centeredOffset);
        }
    }, [rawPoints]);

    useEffect(() => {
        if (!hasAnimated) return;

        const canvas = canvasRef.current;
        if (!canvas || !rawPoints || rawPoints.length === 0) return;

        const ctx = canvas.getContext('2d');
        drawFractal(ctx, rawPoints, scale, offset);
    }, [scale, offset]);

    useEffect(() => {
        return () => {
            cancelAnimationFrame(animationRefs.current.left);
            cancelAnimationFrame(animationRefs.current.right);
        };
    }, []);

    const handleWheel = (e) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();

        const mouseX = e.clientX - rect.left;
        const mouseY = e.clientY - rect.top;

        const zoomFactor = 1.1;
        const newScale = e.deltaY < 0 ? scale * zoomFactor : scale / zoomFactor;

        const newOffset = {
            x: mouseX - ((mouseX - offset.x) * newScale) / scale,
            y: mouseY - ((mouseY - offset.y) * newScale) / scale,
        };

        setScale(newScale);
        setOffset(newOffset);
    };

    const handleMouseDown = (e) => {
        setIsDragging(true);
        setLastPos({ x: e.clientX, y: e.clientY });
    };

    const handleMouseMove = (e) => {
        if (!isDragging) return;
        const dx = e.clientX - lastPos.x;
        const dy = e.clientY - lastPos.y;
        setOffset((prev) => ({ x: prev.x + dx, y: prev.y + dy }));
        setLastPos({ x: e.clientX, y: e.clientY });
    };

    const handleMouseUp = () => setIsDragging(false);

    return (
        <div className="w-full">
            <canvas
                ref={canvasRef}
                width={750}
                height={678}
                style={{
                    cursor: isDragging ? 'grabbing' : 'grab',
                    backgroundColor: 'white',
                }}
                onWheel={handleWheel}
                onMouseDown={handleMouseDown}
                onMouseMove={handleMouseMove}
                onMouseUp={handleMouseUp}
                onMouseLeave={handleMouseUp}
            />
        </div>
    );
};

export default Fractals;