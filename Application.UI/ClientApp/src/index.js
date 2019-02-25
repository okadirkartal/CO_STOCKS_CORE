import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { store } from './_helpers/store';
import 'bootstrap/dist/css/bootstrap.css';
import  { HomePage } from './components/homePage';

import { configureFakeBackEnd } from './_helpers';
//import * as serviceWorker from './serviceWorker';

configureFakeBackEnd();

render (
    <Provider store={store}>
    <HomePage />
    </Provider>,document.getElementById('app')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA

//serviceWorker.unregister();
