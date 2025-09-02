import PlanetBackground from '@/components/PlanetBackground/PlanetBackground';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import React from 'react';
import { Link } from 'react-router-dom';

const InfoPlanet = () => {
    return (
        <div className="relative w-screen h-screen">
            <PlanetBackground />

            <div className="absolute top-1/6 left-25 z-50 text-white w-128">
                <h1 className="text-4xl font-bold mb-4">Згадуємо перший курс</h1>

                <Accordion type="multiple" collapsible className="w-full">
                    <AccordionItem value="item-1">
                        <AccordionTrigger className="text-lg">Лінійна алгебра</AccordionTrigger>
                        <AccordionContent className="text-sm flex flex-col gap-2">
                            <Link to="/info-planet/geom-figures" className="text-blue-400 hover:underline">
                                Геометричні фігури та їх властивості
                            </Link>
                            <Link to="/info-planet/matrixes" className="text-blue-400 hover:underline">
                                Матриці
                            </Link>
                        </AccordionContent>
                    </AccordionItem>

                    <AccordionItem value="item-2">
                        <AccordionTrigger className="text-lg">Аналітична геометрія</AccordionTrigger>
                        <AccordionContent className="text-sm">
                            <Link to="/info-planet/vectors" className="text-blue-400 hover:underline">
                                Вектори
                            </Link>
                        </AccordionContent>
                    </AccordionItem>

                    <AccordionItem value="item-3">
                        <AccordionTrigger className="text-lg">Математичний аналіз</AccordionTrigger>
                        <AccordionContent className="text-sm">
                            <Link to="/info-planet/polynomials" className="text-blue-400 hover:underline">
                                Поліноми
                            </Link>
                        </AccordionContent>
                    </AccordionItem>

                    <AccordionItem value="item-4">
                        <AccordionTrigger className="text-lg">Дискретна математика</AccordionTrigger>
                        <AccordionContent className="text-sm">
                            <Link to="/info-planet/topological-dimension" className="text-blue-400 hover:underline">
                                Топологічна розмірність геометричних елементів
                            </Link>
                        </AccordionContent>
                    </AccordionItem>

                    <AccordionItem value="item-5">
                        <AccordionTrigger className="text-lg">Чисельні методи</AccordionTrigger>
                        <AccordionContent className="text-sm">
                            <Link to="/info-planet/cubic-curves" className="text-blue-400 hover:underline">
                                Криві 3 порядку
                            </Link>
                        </AccordionContent>
                    </AccordionItem>

                    <AccordionItem value="item-6">
                        <AccordionTrigger className="text-lg">Комплексний аналіз</AccordionTrigger>
                        <AccordionContent className="text-sm">
                            <Link to="/info-planet/complex-numbers" className="text-blue-400 hover:underline">
                                Комплексні числа
                            </Link>
                        </AccordionContent>
                    </AccordionItem>
                </Accordion>
            </div>
        </div>
    );
};

export default InfoPlanet;