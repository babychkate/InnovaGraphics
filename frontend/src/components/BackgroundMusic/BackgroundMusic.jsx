import React, { useEffect, useRef } from 'react';

const BackgroundMusic = () => {
    const audioRef = useRef(null);

    useEffect(() => {
        const audio = audioRef.current;

        const handlePlay = () => {
            audio.play().catch((error) => {
                console.warn("Auto-play was prevented:", error);
            });
        };

        document.addEventListener('click', handlePlay, { once: true });

        return () => {
            document.removeEventListener('click', handlePlay);
        };
    }, []);

    return (
        <audio ref={audioRef} loop autoPlay volume={0.3}>
            {/* <source src="/bg-music.mp3" type="audio/mpeg" /> */}
            Your browser does not support the audio element.
        </audio>
    );
};

export default BackgroundMusic;