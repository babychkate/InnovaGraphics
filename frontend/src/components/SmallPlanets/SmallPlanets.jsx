import React from 'react';
import Planet from '../AuthPageComponents/Planet';

const SmallPlanets = () => {
    return (
        <>
            <Planet
                position={[0, 0, -3]} 
                size={1}
                textureUrl="/planet_texture1.jpg" 
                clickable={false}
            />

            <Planet
                position={[-1, 0, 1]} 
                size={2}
                textureUrl="/planet_texture4.jpg" 
                clickable={false}
            />
        </>
    );
}

export default SmallPlanets;