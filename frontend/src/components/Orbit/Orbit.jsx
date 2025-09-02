import React, { useEffect, useRef } from 'react';
import { BufferGeometry, Float32BufferAttribute, LineBasicMaterial } from "three";

const Orbit = ({ radius, numSegments, rotation = { x: 0, y: 0, z: 0 }, position = { x: 0, y: 0, z: 0 }, children }) => {
    const lineRef = useRef();

    useEffect(() => {
        const material = new LineBasicMaterial({
            color: 'white',
            linewidth: 1,
            opacity: 0.2,
            transparent: true
        });

        const geometry = new BufferGeometry();
        const lineLoopPoints = [];

        for (let i = 0; i <= numSegments; i++) {
            const angle = (i / numSegments) * Math.PI * 2;
            const x = radius * Math.cos(angle);
            const z = radius * Math.sin(angle);
            lineLoopPoints.push(x, 0, z);
        }

        geometry.setAttribute('position', new Float32BufferAttribute(lineLoopPoints, 3));

        if (lineRef.current) {
            lineRef.current.geometry = geometry;
            lineRef.current.material = material;

            lineRef.current.rotation.set(rotation.x, rotation.y, rotation.z);

            lineRef.current.position.set(position.x, position.y, position.z);
        }
    }, [radius, numSegments, rotation, position]);

    return (
        <group>
            <lineLoop ref={lineRef} />
            {children}
        </group>
    );
};

export default Orbit;