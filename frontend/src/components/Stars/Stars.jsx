import { PointMaterial, Points } from '@react-three/drei';
import { AdditiveBlending } from "three";
import React, { useMemo } from 'react';

const Stars = ({ count, size }) => {
    const starPositions = useMemo(() => {
        const stars = new Float32Array(count * 3);
        for (let i = 0; i < stars.length; i++) {
            stars[i] = (Math.random() - 0.5) * size;
        }
        return stars;
    }, []);

    return (
        <Points positions={starPositions} stride={3}>
            <PointMaterial
                size={0.02}
                color="white"
                transparent
                opacity={0.9}
                depthWrite={false}
                sizeAttenuation
                blending={AdditiveBlending}
            />
        </Points>
    );
};

export default Stars;