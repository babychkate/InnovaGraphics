import React, { useState } from 'react';
import { useLoader } from "@react-three/fiber";
import { Sphere, useCursor } from "@react-three/drei";
import { TextureLoader } from "three";

const Planet = ({ position, size, textureUrl, clickable = true, rotation = [0, 0, 0], onClick }) => {
    const texture = useLoader(TextureLoader, textureUrl);
    const [hovered, setHovered] = useState(false);
    useCursor(hovered && clickable);

    const handleClick = () => {
        if (clickable) {
            onClick();
        }
    };

    return (
        <Sphere
            args={[1, 32, 32]}
            scale={[size, size, size]}
            position={position}
            rotation={rotation}
            onClick={clickable ? handleClick : undefined}
            onPointerOver={() => clickable && setHovered(true)}
            onPointerOut={() => clickable && setHovered(false)}
        >
            <meshStandardMaterial map={texture} depthWrite={true} />
        </Sphere>
    );
};

export default Planet;