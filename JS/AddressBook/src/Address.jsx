export default function Address(props) {
    return (
        <tr>
            <td>{props.locName}</td>
            <td>{props.street}</td>
            <td>{props.city}, {props.state}, {props.zip}</td>
        </tr>
    );
}