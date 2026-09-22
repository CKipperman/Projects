export default function OperatorBtn({ operator, onClick, className = '' }) {
    return (
        <button className={className} onClick={() => onClick(operator)}>{operator}</button>
    );
}