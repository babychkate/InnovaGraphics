import React, { useRef } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import Planet from '@/components/AuthPageComponents/Planet';
import { ChevronDown } from 'lucide-react'; 

const AboutUsPage = () => {
    const teamRef = useRef(null);

    const handleScroll = () => {
        if (teamRef.current) {
            teamRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    };

    return (
        <div className='w-full pb-[72px]'>
            {/* Секція 1 */}
            <section className='relative w-full h-screen'>
                <Canvas
                    camera={{ position: [1.912, 16.475, 39.856], fov: 50 }}
                    style={{ position: 'absolute', top: 0, left: 0, zIndex: 0 }}
                >
                    <ambientLight intensity={2} />
                    <pointLight position={[-10, -10, -10]} />
                    <Planet position={[0, 24, -10]} size={37} textureUrl="/test-texture.png" />
                    <OrbitControls />
                </Canvas>

                <div className="z-20 flex flex-col items-center justify-between text-center absolute inset-0 py-40">
                    <div className="max-w-3xl text-white">
                        <h1 className='text-3xl font-bold md:text-4xl mb-6'>
                            ХТО МИ І НАВІЩО МИ ЦЕ РОБИМО
                        </h1>
                        <p className='text-xl leading-relaxed'>
                            Ми — команда, яка прагне зробити навчання цікавим, ефективним і натхненним. <br />
                            Наш проєкт поєднує гейміфікацію, креативність та технології, щоб допомогти учням
                            розвиватися у власному темпі, відкривати нові горизонти та досягати більшого.
                        </p>
                    </div>

                    {/* Стрілка вниз */}
                    <button onClick={handleScroll} className="mt-20 animate-bounce text-white">
                        <ChevronDown size={40} />
                    </button>
                </div>
            </section>

            {/* Секція 2 */}
            <section ref={teamRef} className="bg-white px-8">
                <h2 className="text-3xl font-bold text-center mb-16">Наша команда</h2>
                <div className="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-10">
                    <div className="flex flex-col items-center text-center shadow-lg rounded-lg p-6">
                        <img src="/team/roksolana.jpg" alt="Team Member 1" className="w-40 h-40 object-cover rounded-full mb-4" />
                        <h3 className="text-xl font-semibold">Шендюх Роксолана</h3>
                        <p className="text-gray-600">C# backend розробниця, архітекторка системи</p>
                    </div>
                    <div className="flex flex-col items-center text-center shadow-lg rounded-lg p-6">
                        <img src="/team/yaroslav.jpg" alt="Team Member 2" className="w-40 h-40 object-cover rounded-full mb-4" />
                        <h3 className="text-xl font-semibold">Гуз Ярослав</h3>
                        <p className="text-gray-600">Frontend розробник і тестувальник</p>
                    </div>
                    <div className="flex flex-col items-center text-center shadow-lg rounded-lg p-6">
                        <img src="/team/katia.jpg" alt="Team Member 3" className="w-40 h-40 object-cover rounded-full mb-4" />
                        <h3 className="text-xl font-semibold">Бабич Катерина</h3>
                        <p className="text-gray-600">C# backend розробниця, дизайнерка і бізнес аналітикиня</p>
                    </div>
                </div>
            </section>
        </div>
    );
};

export default AboutUsPage;