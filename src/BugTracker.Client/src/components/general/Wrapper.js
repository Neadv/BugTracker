import "./Wrapper.scss";

export const Wrapper = ({ children, title, className, lg, center, ...rest }) => {
    const style = {};
    if (lg){
        style.fontSize = "30px";
    }
    if (center){
        style.textAlign = "center";
    }
    return (
        <div className={`card-wrapper ${className ?? ""}`} {...rest}>
            <div className="card-wrapper-header" style={style}>{title}</div>
            {children}
        </div>
    );
}