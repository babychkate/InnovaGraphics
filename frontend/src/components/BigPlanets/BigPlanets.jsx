import React from 'react'
import Planet from '../AuthPageComponents/Planet'

const BigPlanets = () => {
    return (
        <>
            <Planet
                position={[4, 2, 1.5]}
                size={3}
                rotation={[0.3, -1, -0.5]}
                textureUrl="/test-texture.png"
                clickable={false}
            />

            <Planet
                position={[-2.75, -1.75, 2]}
                size={2}
                rotation={[2, -1, -1]}
                textureUrl="/test-texture.png"
                clickable={false}
            />
        </>
    )
}

export default BigPlanets