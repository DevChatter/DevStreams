module.exports = {
    outputDir: 'wwwroot/vue',
    publicPath: "/",
    chainWebpack: config => {
        // aspnet uses the other hmr so remove this one
        config.plugins.delete('hmr');
    }
}