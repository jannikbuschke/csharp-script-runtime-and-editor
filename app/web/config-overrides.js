const path = require('path')
const { override, fixBabelImports } = require('customize-cra')
const { addReactRefresh } = require('customize-cra-react-refresh')
const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = override(
    fixBabelImports('antd', {
        libraryName: 'antd',
        libraryDirectory: 'es',
        style: 'css',
    }),
    fixBabelImports('formik-antd',
        {
            libraryName: 'formik-antd',
            libraryDirectory: 'es',
            style: "css",
        },
    ),
    addReactRefresh({ disableRefreshCheck: true })
);

// module.exports = function override(config, env) {
//   if (!config.plugins) {
//     config.plugins = [];
//   }
//   config.plugins.push(
//     new MonacoWebpackPlugin()
//   );
//   return config;
// }