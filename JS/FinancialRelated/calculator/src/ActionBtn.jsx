export default function ActionBtn({ action, onClick, className = '' }) {
    return (
        <button className={className} onClick={() => onClick(action)}>{action}</button>
    );
}