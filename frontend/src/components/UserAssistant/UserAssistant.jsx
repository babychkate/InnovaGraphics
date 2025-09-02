import React, { useRef, useEffect } from 'react';

const UserAssistant = ({ top, right, bottom, left, text }) => {
    const hasSpokenRef = useRef(false); 

    useEffect(() => {
        const speakOnce = () => {
            const voices = window.speechSynthesis.getVoices();

            if (voices.length === 0) {
                window.speechSynthesis.onvoiceschanged = () => {
                    if (!hasSpokenRef.current) {
                        speakOnce(); 
                    }
                };
                return;
            }

            if (!hasSpokenRef.current) {
                const synth = window.speechSynthesis;
                const utterance = new SpeechSynthesisUtterance(text);

                let selectedVoice = voices.find(v => 
                    v.lang === "en-US" && v.name.toLowerCase().includes("female")
                );

                if (!selectedVoice) {
                    selectedVoice = voices.find(v =>
                        v.lang === "en-US" && (
                            v.name.toLowerCase().includes("susan") ||
                            v.name.toLowerCase().includes("samantha") ||
                            v.name.toLowerCase().includes("zoe") ||
                            v.name.toLowerCase().includes("zira")
                        )
                    );
                }

                if (!selectedVoice) {
                    selectedVoice = voices.find(v => v.lang === "en-US") || voices[0];
                }

                utterance.voice = selectedVoice;

                synth.speak(utterance);

                utterance.onend = () => {
                    hasSpokenRef.current = false; 
                };

                hasSpokenRef.current = true; 
            }
        };

        speakOnce();
    }, [text]);  

    return (
        <div 
            className="absolute flex items-center gap-4"
            style={{
                top: top || 'auto',       
                right: right || 'auto',   
                bottom: bottom || 'auto', 
                left: left || 'auto',     
            }}
        >
            <div className='bg-blue-300 p-3 rounded-full shadow-md'>
                <p className="text-lg font-medium text-gray-800">
                    {text}
                </p>
            </div>
            <img 
                src="/user_asis.png" 
                alt="Instructor" 
                className="w-16 h-16 rounded-full object-cover"
            />
        </div>
    );
};

export default UserAssistant;