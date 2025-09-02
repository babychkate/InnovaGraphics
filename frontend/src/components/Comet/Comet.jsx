import * as THREE from 'three';
import React, { useRef, useEffect } from 'react';
import { AdditiveBlending } from 'three';

const Comet = ({ position }) => {
    const particlesRef = useRef();

    useEffect(() => {
        const particleCount = 500;
        const positions = new Float32Array(particleCount * 3);
        const colors = new Float32Array(particleCount * 3);

        const tailLength = 7; 
        const startRadius = 0.5;
        const maxRadius = 1.5; 
        const coreOffset = -1; 

        for (let i = 0; i < particleCount; i++) {
            const t = Math.random();
            const z = -(coreOffset + t * (tailLength - coreOffset));

            const radius = startRadius + (t ** 1.5) * (maxRadius - startRadius);

            const angle = Math.random() * Math.PI * 2;
            const x = Math.cos(angle) * radius;
            const y = Math.sin(angle) * radius;

            positions[i * 3] = x;
            positions[i * 3 + 1] = y;
            positions[i * 3 + 2] = z;

            colors[i * 3] = 1; 
            colors[i * 3 + 1] = t;
            colors[i * 3 + 2] = 0; 
        }

        const geometry = particlesRef.current.geometry;
        geometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));
        geometry.setAttribute('color', new THREE.BufferAttribute(colors, 3));

        geometry.attributes.position.needsUpdate = true;
        geometry.attributes.color.needsUpdate = true;
    }, []);

    return (
        <group position={position}>
            <mesh position={[0, 0, 1]}>
                <sphereGeometry args={[0.5, 32, 32]} />
                <meshStandardMaterial 
                    color={'orange'} 
                    emissive={'red'} 
                    emissiveIntensity={3} 
                />
            </mesh>

            <points ref={particlesRef}>
                <bufferGeometry attach="geometry" />
                <pointsMaterial 
                    size={0.12} 
                    blending={AdditiveBlending} 
                    transparent={true} 
                    opacity={0.8} 
                    vertexColors={true} 
                />
            </points>
        </group>
    );
};

export default Comet;