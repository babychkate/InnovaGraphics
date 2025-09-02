import React from 'react';
import { Accordion, AccordionItem, AccordionTrigger, AccordionContent } from "@/components/ui/accordion";

const FAQPage = () => {
    return (
        <div className="max-w-3xl mx-auto pt-[100px] px-4">
            <h1 className="text-3xl font-bold text-center mb-10">Поширені запитання</h1>
            <Accordion type="multiple" className="w-full space-y-4">
                <AccordionItem value="q1">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Що це за платформа?
                    </AccordionTrigger>
                    <AccordionContent>
                        Це освітня платформа з гейміфікацією, яка допомагає учням вчитися цікаво та ефективно.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q2">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Для кого створений ваш проєкт?
                    </AccordionTrigger>
                    <AccordionContent>
                        Для школярів, студентів, викладачів і всіх, хто хоче вивчати нове у зручній формі.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q3">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Чи безкоштовне користування платформою?
                    </AccordionTrigger>
                    <AccordionContent>
                        Так, базові функції є безкоштовними. Деякі розширені можливості можуть бути платними.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q4">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Як зареєструватися?
                    </AccordionTrigger>
                    <AccordionContent>
                        Натисніть кнопку "Реєстрація" у верхньому правому куті та заповніть коротку форму.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q5">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Які предмети доступні для вивчення?
                    </AccordionTrigger>
                    <AccordionContent>
                        Математика, програмування, історія, фізика та інші. Постійно додаються нові курси.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q6">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Чи є мобільна версія платформи?
                    </AccordionTrigger>
                    <AccordionContent>
                        Так, наш сайт адаптований під мобільні пристрої. Також ми плануємо окремий застосунок.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q7">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Чи можна створювати власні курси?
                    </AccordionTrigger>
                    <AccordionContent>
                        Так, викладачі можуть створювати власні курси й завдання для учнів.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q8">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Яка команда стоїть за проєктом?
                    </AccordionTrigger>
                    <AccordionContent>
                        Команда ентузіастів: backend, frontend розробники, дизайнер та аналітики.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q9">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Як зв'язатися з підтримкою?
                    </AccordionTrigger>
                    <AccordionContent>
                        Напишіть нам на support@yourplatform.com або скористайтеся формою зворотного зв'язку.
                    </AccordionContent>
                </AccordionItem>

                <AccordionItem value="q10">
                    <AccordionTrigger className="text-lg md:text-xl font-semibold text-primary">
                        Чи будуть оновлення платформи?
                    </AccordionTrigger>
                    <AccordionContent>
                        Так, ми постійно покращуємо функціональність і додаємо нові можливості.
                    </AccordionContent>
                </AccordionItem>
            </Accordion>
        </div>
    );
};

export default FAQPage;