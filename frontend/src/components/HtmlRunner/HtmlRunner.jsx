import React, { useEffect, useRef } from 'react';

const HtmlRunner = ({ content }) => {
  const containerRef = useRef(null);

  useEffect(() => {
    if (!containerRef.current || !content) return;

    containerRef.current.innerHTML = '';

    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = content;

    Array.from(tempDiv.querySelectorAll('script')).forEach((oldScript) => {
      const newScript = document.createElement('script');

      if (oldScript.src) {
        newScript.src = oldScript.src;
      } else {
        const wrappedContent = `(function(){\n${oldScript.textContent}\n})();`;
        newScript.textContent = wrappedContent;
      }

      Array.from(oldScript.attributes).forEach(attr =>
        newScript.setAttribute(attr.name, attr.value)
      );

      oldScript.replaceWith(newScript);
    });

    while (tempDiv.firstChild) {
      containerRef.current.appendChild(tempDiv.firstChild);
    }
  }, [content]);

  return <div ref={containerRef} />;
};

export default HtmlRunner;