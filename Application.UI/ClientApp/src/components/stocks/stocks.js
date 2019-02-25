import React from '../../../../../../Library/Caches/typescript/2.9/node_modules/@types/react'; 
import { FaPlusCircle ,FaTrash , FaPen } from 'react-icons/fa';

export default class Stocks extends React.Component {
   
    constructor(props) {
       super(props);
       this.state = { stockData : []};

       this.addStock = this.addStock.bind(this);
    }

    componentDidMount(){
       /* fetch('/api/Stocks/Get')
        .then(res => res.json()) 
        .then(
            (result) => {
                this.setState({authorData : result});
            },
            (error) => {
                this.setState({ error});
            }
        )*/
        this.setState({stockData : [{ id:"1",code:"01",name:"Pencil",price:"12.3",piece:5,isActive:true}]});
    }

    addStock(id) {
        this.props.history.push("/addStock");
    }

    render(){
        return (
          <div className="container">
            <div className="table-wrapper">
                <div className="row">
                    <div className="col-sm-6">
                        <h2>Manage <b>Stocks</b></h2>
                    </div>
                    <div className="col-sm-6">
                     <a onClick={(id)=>this.addStock(0)} className="btn btn-success">
                     <i className="material-icons"><FaPlusCircle /></i><span>Add New Stock</span>
                     </a>
                    </div>
                </div>
            </div>

            <table className="table table-striped table-hover">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Code</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Piece</th>
                    <th>Is Active</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {this.state.stockData.map((item,i)=> {
                    return (
                        <tr key={i}>
                        <td>{item.id}</td>
                        <td>{item.code}</td>
                        <td>{item.name}</td>
                        <td>{item.price}</td>
                        <td>{item.piece}</td>
                        <td>{item.isActive}</td>
                        <td><i className="material-icons"><FaPlusCircle /></i><span>Edit</span></td>
                        <td><i className="material-icons"><FaTrash /></i><span>Delete</span></td>
                        </tr>
                    )
                })}
            </tbody>
            </table>

          </div>
        );
    }
}