import { Component } from 'react';
import DigitBtn from './DigitBtn';
import OperatorBtn from './OperatorBtn';
import ActionBtn from './ActionBtn';
import './App.css'

export default class App extends Component {
  state = {
    current: '0',
    operator: null,
    total: null,
    needSecondNum: false
  }

  handleNumber = (num) => {
    const { current, needSecondNum } = this.state;

    if (needSecondNum) {
      this.setState({
        current: String(num),
        needSecondNum: false
      });
    } else {
      this.setState({
        current: current === '0' ? String(num) : current + num
      });
    }
  };

  handleOperator = (op) => {
    const { current, operator, total } = this.state;

    if (op === '=') {
      if (total !== null && operator) {
        const result = this.calculate(
          Number(total), Number(current), operator
        );
        this.setState({
          current: String(result),
          total: null,
          operator: null,
        });
      }
      return;
    }

    this.setState({
      total: current,
      operator: op,
      needSecondNum: true
    });
  };

  handleDecimal = () => {
    const { current, needSecondNum } = this.state;
    if (current.includes('.')) return;

    if (needSecondNum) {
      this.setState({
        current: '0.',
        needSecondNum: false
      });
    } else {
      this.setState({
        current: current + '.'
      });
    }
  };

  handleClear = () => {
    this.setState({
      current: '0',
      total: null,
      operator: null,
      needSecondNum: false
    });
  };

  handleDelete = () => {
    const { current } = this.state;

    if (current.length === 1 ) {
      this.setState({ current: '0' });
    } else {
      this.setState({ current: current.slice(0, -1) });
    }
  };

  calculate = (a, b, op) => {
    switch (op) {
      case '+': return a + b;
      case '-': return a - b;
      case '*': return a * b;
      case '÷': return b === 0 ? 'Error' : a / b;
      default: return b;
    }
  };

  render() {
    const { current, operator, total, needSecondNum } = this.state;
    return (
      <div className='calculator'>
        <div className='display'>
          <div className='previousOperand'>
            {this.state.total !== null && (
              <>
                {total} {operator || ''}{' '}
                {needSecondNum ? '' : current}
                {operator === '=' ? ' =' : ''}
              </>
            )}
          </div>
          <div className='currentOperand'>
            {current}
          </div>
        </div>
        <ActionBtn className='span2' action='AC' onClick={this.handleClear}/>
        <ActionBtn action='DEL' onClick={this.handleDelete}/>
        <OperatorBtn operator='÷' onClick={this.handleOperator} />
        <DigitBtn digit={1} onClick={this.handleNumber} />
        <DigitBtn digit={2} onClick={this.handleNumber} />
        <DigitBtn digit={3} onClick={this.handleNumber} />
        <OperatorBtn operator='*' onClick={this.handleOperator} />
        <DigitBtn digit={4} onClick={this.handleNumber} />
        <DigitBtn digit={5} onClick={this.handleNumber} />
        <DigitBtn digit={6} onClick={this.handleNumber} />
        <OperatorBtn operator='+' onClick={this.handleOperator} />
        <DigitBtn digit={7} onClick={this.handleNumber} />
        <DigitBtn digit={8} onClick={this.handleNumber} />
        <DigitBtn digit={9} onClick={this.handleNumber} />
        <OperatorBtn operator='-' onClick={this.handleOperator} />
        <ActionBtn action='.' onClick={this.handleDecimal} />
        <DigitBtn digit={0} onClick={this.handleNumber} />
        <OperatorBtn className='span2' operator='=' onClick={this.handleOperator} />
      </div>
    )
  }
}
