export default function DigitBtn({digit, onClick}) {
    return (
        <button onClick={() => onClick(digit)}>{digit}</button>
    );
}