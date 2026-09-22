import './ColorPicker.css'
import { useState } from "react";

const FONT_OPTIONS = [
    { value: 'Arial, sans-serif', label: 'Arial' },
    { value: 'Georgia, serif', label: 'Georgia' },
    { value: '"Times New Roman", Times, serif', label: 'Times New Roman' },
    { value: '"Courier New", Courier, monospace', label: 'Courier New' },
    { value: '"Comic Sans MS", cursive', label: 'Comic Sans' }
];

export default function ColorPicker({ content }) {
    const [bgColor, setBgColor] = useState('white');
    const [textColor, setTextColor] = useState('black');
    const [fontFamily, setFontFamily] = useState(FONT_OPTIONS[0].value);

    const handleColorChange = (e) => {
        const value = e.target.value;
        const type = e.target.name;

        if (type === 'background') {
            setBgColor(value);
        } else if (type === 'text') {
            setTextColor(value);
        }
    };

    const handleFontChange = (e) => {
        setFontFamily(e.target.value);
    };

    const coloredContent = (
        <div className='content' style={{ backgroundColor: bgColor, color: textColor, fontFamily }}>
            {content}
        </div>
    );

    return (
        <>
            <div className="controls">
                <h2>Controls:</h2>
                <label>Background Color:
                    <input type="color" name='background' value={bgColor} onChange={handleColorChange} />
                </label>
                <label>Text Color:
                    <input type="color" name='text' value={textColor} onChange={handleColorChange} />
                </label>
                <label>Font:
                    <select value={fontFamily} onChange={handleFontChange}>
                        {FONT_OPTIONS.map(font => (
                            <option key={font.value} value={font.value}>
                                {font.label}
                            </option>
                        ))}
                    </select>
                </label>
            </div>
            {coloredContent}
        </>
    );
}