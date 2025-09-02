import Planet from '@/components/AuthPageComponents/Planet';
import Comet from '@/components/Comet/Comet';
import Orbit from '@/components/Orbit/Orbit';
import SpaceStation from '@/components/SpaceStation/SpaceStation';
import Stars from '@/components/Stars/Stars';
import { OrbitControls, Text } from '@react-three/drei';
import { Canvas, useFrame } from '@react-three/fiber';
import React from 'react';
import { useNavigate } from 'react-router-dom';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const objectsData = [
    {
        orbit: { radius: 11, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: 0, position: [0, -4.5, 20], size: 5.5, textureUrl: "/sun_texture2.jpg", clickable: true },
        text: null,
    },
    {
        orbit: { radius: 18.5, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: "EA8E4C69-699A-4A23-8554-5CEDB72B635B", position: [5.5, -2, 10.5], size: 2, textureUrl: "/planet_texture/blue1.jpg" },
        text: {
            position: [7, -0.5, 12.5],
            fontSize: 0.5,
            color: "white",
            rotation: [0, Math.PI / 4, 0],
            value: "Ознайомча планета",
        },
    },
    {
        orbit: { radius: 28, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: "03A146EB-D2D9-4283-BC2B-031F026AA7FA", position: [-15, 2.5, 12], size: 5, textureUrl: "/planet_texture/green.jpeg" },
        text: {
            position: [-10, 3, 13.5],
            fontSize: 1,
            color: "white",
            rotation: [0, Math.PI / 2, 0],
            value: "Лабораторна №1",
        },
    },
    {
        orbit: { radius: 36, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: "AC4B8F5B-DB7B-44BA-96B2-6AE26BC85F3C", position: [17, 0, 0], size: 3.25, textureUrl: "/planet_texture/r1.jpg" },
        text: {
            position: [15.5, 0.5, 3.5],
            fontSize: 1,
            color: "white",
            rotation: [0, -Math.PI / 4, 0],
            value: "Лабораторна №2",
        },
    },
    {
        orbit: { radius: 6.5, rotation: { x: 0, y: -0.3, z: -0.85 }, position: { x: 17, y: 0, z: 0 } },
        planet: { id: 4, position: [13, 4.5, 0], size: 1, textureUrl: "/planet_texture/shop.jpeg", to: "/shop" },
        text: {
            position: [13, 4.5, 1.4],
            fontSize: 0.5,
            color: "white",
            rotation: [0, -Math.PI / 6, 0],
            value: "Магазин",
        },
    },
    {
        orbit: { radius: 48.5, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: "3895D417-4178-4E3D-80AC-7DA8C1785899", position: [0, 10, -12], size: 3.4, textureUrl: "/planet_texture/purple.jpeg" },
        text: {
            position: [1.5, 10, -8],
            fontSize: 1,
            color: "white",
            rotation: [0, Math.PI / 6, 0],
            value: "Лабораторна №3",
        },
    },
    {
        orbit: { radius: 71, rotation: { x: 0, y: -1, z: -0.5 }, position: { x: 0, y: -4, z: 20 } },
        planet: { id: "2DEF880A-D118-4D82-B00C-18C6E156225A", position: [-40, 16, 0], size: 4, textureUrl: "/planet_texture/colorful.jpeg" },
        text: {
            position: [-34.5, 15.5, 2],
            fontSize: 1.25,
            color: "white",
            rotation: [Math.PI / 16, Math.PI / 2.5, 0],
            value: "Лабораторна №4",
        },
    },
    {
        orbit: null,
        planet: { id: "A30E1F22-6D93-4983-9CDC-B6F8902E8730", position: [22.5, 18, -43.5], size: 4, textureUrl: "/planet_texture/y2.jpeg" },
        text: {
            position: [22, 18, -38.5],
            fontSize: 1.5,
            color: "white",
            rotation: [0, 0, -Math.PI / 32],
            value: "Лабораторна №5",
        },
    },
    {
        orbit: null,
        planet: null,
        spaceStation: {
            position: [0, 3, 2.25],
            scale: 2.5,
            rotation: [Math.PI / 10, 0, -Math.PI / 10],
            to: "/multiplayer",
            clickable: true,
        },
        text: {
            position: [0, 5.75, 2.25],
            fontSize: 1.25,
            color: "white",
            rotation: [0, Math.PI / 4, -Math.PI / 32],
            value: "Мультиплеєр",
        },
    },
];

const HomePage = () => {
    const navigate = useNavigate();

    const handlePlanetClick = (planetId, planetText) => {
        if (planetId === 0) return;
        
        if (planetText === "Ознайомча планета") {
            navigate("/info-planet");
        } else if (planetId === 4) {
            navigate("/shop");
        } else {
            navigate(`/planets/${planetId}`);
        }
    };

    return (
        <div className='w-screen h-screen bg-black'>
            <Canvas
                camera={{
                    position: [13.816328849321339, 4.920889564384977, 21.52252220153435],
                    fov: 75,
                }}
            >
                <ambientLight intensity={0.3} />
                <directionalLight position={[10, 10, 10]} intensity={2} />

                <Stars count={15000} size={1000} />

                {objectsData.map((object, index) => {
                    const { orbit, planet, text, spaceStation } = object;

                    const content = (
                        <>
                            {planet && (
                                <Planet
                                    id={planet.id}
                                    position={planet.position}
                                    size={planet.size}
                                    textureUrl={planet.textureUrl}
                                    onClick={() => handlePlanetClick(planet?.id, text?.value)}
                                />
                            )}

                            {spaceStation && (
                                <SpaceStation
                                    position={spaceStation.position}
                                    scale={spaceStation.scale}
                                    rotation={spaceStation.rotation}
                                    to={spaceStation.to}
                                    clickable={spaceStation.clickable}
                                />
                            )}

                            {text && (
                                <Text
                                    position={text.position}
                                    fontSize={text.fontSize}
                                    color={text.color}
                                    rotation={text.rotation}
                                >
                                    {text.value}
                                </Text>
                            )}
                        </>
                    );

                    return orbit ? (
                        <Orbit
                            key={index}
                            radius={orbit.radius}
                            numSegments={100}
                            rotation={orbit.rotation}
                            position={orbit.position}
                        >
                            {content}
                        </Orbit>
                    ) : (
                        <React.Fragment key={index}>
                            {content}
                        </React.Fragment>
                    );
                })}

                <CameraLogger />
                <OrbitControls />
            </Canvas>
        </div>
    );
};

export default HomePage;