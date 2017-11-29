const webpack = require('webpack');
const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const output = "wwwroot";

module.exports = {
    context: path.resolve(__dirname),
    entry: {
        'bundle': './Scripts/app.js'
    },
    output: {
        path: path.join(__dirname, output),
        filename: '[name].js'
    },
    resolve: {
        extensions: ['*', '.js', '.json']
    },
    module: {
        loaders: [
            {
                test: /\.js?$/, loader: 'babel-loader', exclude: /node_modules/,
                query: {
                    presets: ['es2015', 'react']
                }
            },
            { test: /\.css$/, loaders: ['css'] }
        ]
    },
    plugins: [
        new CopyWebpackPlugin([
            { from: './node_modules/graphiql/graphiql.css', to: 'graphiql.css' },
            { from: './index.html', to: 'index.html' }
        ])
    ]
}